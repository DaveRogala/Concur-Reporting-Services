using ConcurExpense;
using ConcurExpense.Models;
using ConcurReporting.Domain.Models;
using ConcurReporting.Domain.Services.Interfaces;
using ConcurReporting.Helpers;
using ConcurReporting.Mappers;
using ConcurReportingDatabaseServices.Models;
using ConcurReportingDatabaseServices.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcurReporting;

internal class ConcurReportClient : IConcurReportClient
{
    private bool disposedValue;
    private readonly IConcurExpenseClient _expenseClient;
    private readonly IReportServices _reportServices;
    private readonly IQueryHistoryServices _queryHistoryServices;
    private readonly ILogger<ConcurReportClient> _logger;
    public ConcurReportClient(IConcurExpenseClient expenseClient, 
                                IReportServices reportServices, 
                                IQueryHistoryServices queryHistoryServices,
                                ILogger<ConcurReportClient> logger)
    {
        _expenseClient = expenseClient;
        _reportServices = reportServices;
        _queryHistoryServices = queryHistoryServices;

        _logger = logger;        
    }
    public async Task<int> ProcessEntities(DateTime utcNow, DateTime startDate, DateTime? endDate = null)
    {
        try
        {

            int pageSize = 100;
            var returnedReportDtos = await _expenseClient.GetReportsAsync(limit: 100, modifiedDateAfter: startDate, modifiedDateBefore: endDate);                   
            int updates = 0;
            int pages = (int)Math.Ceiling(returnedReportDtos.Count /(decimal) pageSize);

            for (int i = 0; i < pages; i++)
            {
                _logger.LogInformation($"Start Date: {startDate}, End Date: {endDate}. Page {i+1} of {pages} pages");

                updates += await ProcessPage(returnedReportDtos.Skip(i * pageSize).Take(pageSize), utcNow);                
            }
            return updates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProcessEntities failed. Message: {msg}", ex.Message);
            throw;
        }
    }
    private async Task<int> ProcessPage(IEnumerable<ReportDto> reportDtos, DateTime utcNow)
    {      
        var existingReports = await _reportServices.FindReportsAsync(r => reportDtos.Select(d => d.ID).Contains(r.ConcurID));
        foreach (var reportDto in reportDtos)
        {   
            Report? existingReport = existingReports.FirstOrDefault(r => r.ConcurID == reportDto.ID);
            
            //Check the LastModifiedDateTime to handle any overlap scenarios
            if (existingReport is null || reportDto.LastModifiedDate > existingReport.LastModifiedDateTimeUtc)
            {
                List<EntryDto> entryDtos = await _expenseClient.GetEntriesAsync(reportDto.ID, limit: 100);

                //Entries can be moved from one Report to another
                //First remove any entries that are associated with other reports

                var reportsWithEntries = await _reportServices.FindReportsAsync(r => r.ConcurID != reportDto.ID
                                                                                        && r.Entries.Any(e => entryDtos.Select(d => d.ID).Contains(e.ConcurID)));

                foreach(Report report in reportsWithEntries)
                {
                    var entriesToRemove = report.Entries.Where(e => entryDtos.Select(d => d.ID).Contains(e.ConcurID)).ToList();
                    
                    for (int i = 0; i < entriesToRemove.Count; i++)                    
                    {
                        report.Entries.Remove(entriesToRemove[i]);
                    }
                }
                
                List<ItemizationDto> itemizationDtos = entryDtos.Any(e => e.HasItemizations)
                    ? await _expenseClient.GetItemizationsAsync(reportDto.ID, limit: 100)
                    : [];

                List<AllocationDto> allocationDtos = await _expenseClient.GetAllocationsAsync(reportDto.ID, limit: 100);

                // Filter allocations to only those with a valid parent ConcurID.
                // Concur can enter an inconsistent state where an allocation's EntryID does not
                // match any current entry or itemization. Saving such an allocation would produce
                // an orphaned row (null EntryId AND null ItemizationId), and a subsequent sync
                // that reactivates the same ConcurID would then fail on the unique index.
                var validParentIds = new HashSet<string>(
                    entryDtos.Select(e => e.ID).Concat(itemizationDtos.Select(i => i.ID)));
                allocationDtos = FilterValidAllocations(allocationDtos, validParentIds, reportDto.ID);

                if (existingReport is null)
                {
                    Report report = reportDto.ReportFromDto(utcNow);

                    report.Entries = entryDtos.Select(e => GetAddEntry(e, allocationDtos, itemizationDtos, utcNow)).ToList();

                    await _reportServices.AddReportAsync(report);
                }
                else 
                {
                    //Update the report if it already exists
                    
                    var joinedEntries = existingReport.Entries.ToJoinedEntities(entryDtos);

                    foreach (var joinedEntry in joinedEntries)
                    {

                        if (joinedEntry.Entity is null && joinedEntry.Dto is not null)                            
                        {                            
                            existingReport.Entries.Add(GetAddEntry(joinedEntry.Dto, allocationDtos, itemizationDtos, utcNow));
                        }
                        else if (joinedEntry.Entity is not null && joinedEntry.Dto is not null && joinedEntry.Dto.LastModified > joinedEntry.Entity.LastModifiedDateTimeUtc)
                        {
                            joinedEntry.Entity = UpdateEntry(joinedEntry.Entity, joinedEntry.Dto, allocationDtos, itemizationDtos, utcNow);
                        }
                        else if (joinedEntry.Entity is not null && joinedEntry.Dto is null)
                        {
                            existingReport.Entries.Remove(joinedEntry.Entity);
                        }
                    }
                    _reportServices.UpdateReport(existingReport.UpdateReportFromDto(reportDto, utcNow));
                }
            }
            
        }
        return await _reportServices.SaveChangesAsync();
    }
    private List<AllocationDto> FilterValidAllocations(List<AllocationDto> allocationDtos, HashSet<string> validParentIds, string reportId)
    {
        var valid = new List<AllocationDto>(allocationDtos.Count);
        foreach (var dto in allocationDtos)
        {
            if (string.IsNullOrEmpty(dto.EntryID) || !validParentIds.Contains(dto.EntryID))
            {
                _logger.LogWarning(
                    "Allocation {AllocationId} for report {ReportId} has EntryID '{EntryId}' that does not match any known entry or itemization ConcurID. Skipping to prevent orphan.",
                    dto.ID, reportId, dto.EntryID ?? "(null)");
            }
            else
            {
                valid.Add(dto);
            }
        }
        return valid;
    }
    private List<Allocation> UpdateAllocations(List<Allocation> allocations, List<AllocationDto> allocationDtos, string concurId, DateTime utcNow)
    {
        var joinedAllocations = allocations.ToJoinedEntities(allocationDtos.Where(a => a.EntryID == concurId).ToList());

        foreach (var joinedAllocation in joinedAllocations)
        {
            if (joinedAllocation.Entity is null && joinedAllocation.Dto is not null)
            {
                allocations.Add(joinedAllocation.Dto.AllocationFromDto(utcNow));
            }
            else if (joinedAllocation.Entity is not null && joinedAllocation.Dto is not null)
            {
                joinedAllocation.Entity = joinedAllocation.Entity.UpdateAllocationFromDto(joinedAllocation.Dto, utcNow);
            }
            else if (joinedAllocation.Entity is not null && joinedAllocation.Dto is null)
            {
               allocations.Remove(joinedAllocation.Entity);
            }            
        }
        return allocations;
    }
    private Entry UpdateEntry(Entry entry, EntryDto dto, List<AllocationDto> allocationDtos, List<ItemizationDto> itemizationDtos, DateTime utcNow)
    {
        List<ItemizationDto> entryItemizationDtos = itemizationDtos.Where(e => e.EntryID == entry.ConcurID).ToList();

        // When all itemizations are being removed, clear their allocations BEFORE updating
        // entry-level allocations. If EF Core uses SetNull (optional FK) rather than Cascade on
        // Allocation→Itemization, removing the itemizations would orphan their allocations in the
        // same SaveChanges call where the entry-level allocations (potentially sharing the same
        // ConcurIDs) are inserted, causing a unique-index conflict.
        if ((entry.Itemizations?.Count ?? 0) > 0 && entryItemizationDtos.Count == 0)
        {
            foreach (var itemization in entry.Itemizations!)
            {
                itemization.Allocations?.Clear();
            }
            entry.Itemizations = null;
        }

        entry.Allocations = UpdateAllocations(entry.Allocations, allocationDtos, entry.ConcurID, utcNow);

        if ((entry.Itemizations?.Count ?? 0) == 0 && entryItemizationDtos.Count > 0)
        {
            // if there are no existing itemizations and there are new ones, add them as new
            List<Itemization> itemizations = itemizationDtos.Where(e => e.EntryID == dto?.ID).Select(e => e.ItemizationFromDto(utcNow)).ToList();
            foreach (Itemization itemization in itemizations)
            {
                itemization.Allocations = allocationDtos.Where(e => e.EntryID == itemization.ConcurID).Select(e => e.AllocationFromDto(utcNow)).ToList();
            }
            entry.Itemizations = itemizations;
        }
        else if (entry.Itemizations is not null && entry.Itemizations.Count > 0 && entryItemizationDtos.Count > 0)
        {
            //There are existing and updated itemizations
            var joinedItemizations = entry.Itemizations.ToJoinedEntities(entryItemizationDtos);

            foreach (var joinItemization in joinedItemizations)
            {
                if(joinItemization.Entity is null && joinItemization.Dto is not null)
                {
                    //Add new itemization
                    Itemization newItemization = joinItemization.Dto.ItemizationFromDto(utcNow);
                    newItemization.Allocations = allocationDtos.Where(e => e.EntryID == newItemization.ConcurID).Select(e => e.AllocationFromDto(utcNow)).ToList();
                    entry.Itemizations.Add(newItemization);
                }
                if(joinItemization.Entity is not null && joinItemization.Dto is not null)
                {
                    // Always sync allocations regardless of whether the itemization itself changed.
                    // Concur may update an allocation without bumping the parent itemization's
                    // LastModified, so gating allocation sync on that timestamp can silently drop changes.
                    joinItemization.Entity.Allocations = UpdateAllocations(joinItemization.Entity.Allocations, allocationDtos, joinItemization.Entity.ConcurID, utcNow);

                    if(joinItemization.Dto.LastModified > joinItemization.Entity.LastModifiedDateTimeUtc)
                    {
                        joinItemization.Entity = joinItemization.Entity.UpdateItemizationFromDto(joinItemization.Dto, utcNow);
                    }
                }
                if(joinItemization.Entity is not null && joinItemization.Dto is null)
                {
                    //Delete an itemization
                    entry.Itemizations.Remove(joinItemization.Entity);
                }
            }
        }

        return entry.UpdateEntryFromDto(dto, utcNow);
    }
    private Entry GetAddEntry(EntryDto dto, List<AllocationDto> allocationDtos, List<ItemizationDto> itemizationDtos, DateTime utcNow)
    {        
        Entry entry = dto.EntryFromDto(utcNow);
        List<Itemization> itemizations = itemizationDtos.Where(e => e.EntryID == dto?.ID).Select(e => e.ItemizationFromDto(utcNow)).ToList();
        entry.Allocations = allocationDtos.Where(e => e.EntryID == entry.ConcurID).Select(e => e.AllocationFromDto(utcNow)).ToList();
        if (itemizations.Count > 0)
        {
            // An Entry may not have itemizations
            // If it does then the allocation is tied the itemization through the allocation.EntryID == itemization.ConcurID
            foreach (Itemization itemization in itemizations)
            {
                itemization.Allocations = allocationDtos.Where(e => e.EntryID == itemization.ConcurID).Select(e => e.AllocationFromDto(utcNow)).ToList();
            }
            entry.Itemizations = itemizations;
        }
        return entry;
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                _reportServices.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~ConcurReportClient()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async Task<int> ProcessEntities(DateTime? startDate = null)
    {
        try
        {
            var lastUpdated = await _queryHistoryServices.GetLastQueryHistoryAsync();
            int entityCount = 0;            
            
            //Subtracting 10 seconds from updated to datetime to prevent edge scenarios

            DateTime startDateTime = lastUpdated?.DateTimeUpdatedUtc.AddSeconds(-10) ?? startDate ?? new DateTime(2022, 1, 1);            

            while(startDateTime < DateTime.UtcNow)
            {
                DateTime endDateTime = startDateTime.AddDays(7);
                DateTime utcNow = DateTime.UtcNow;
                entityCount = await ProcessEntities(utcNow, startDateTime, endDateTime);

                await _queryHistoryServices.AddQueryHistoryAsync(new QueryHistory
                {
                    DateTimeAddedUtc = utcNow,
                    DateTimeUpdatedUtc = endDateTime > utcNow ? utcNow : endDateTime,
                    IsSuccess = true,
                    Message = $"StartDateTime: {startDateTime}, EndDateTime: {endDateTime}"
                });
                await _queryHistoryServices.SaveChangesAsync();

                startDateTime = endDateTime.AddSeconds(-10);
            }
            return entityCount;
        }
        catch (Exception ex)
        {
            try
            {
                await _queryHistoryServices.AddQueryHistoryAsync(new QueryHistory
                {
                    DateTimeAddedUtc = DateTime.UtcNow,
                    DateTimeUpdatedUtc = DateTime.UtcNow,
                    IsSuccess = false,
                    Message = ex.Message
                });
                await _queryHistoryServices.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            
            _logger.LogError(ex, "Process Entities failed.  Message: {msg}", ex.Message);
            throw;
        }
    }
}
