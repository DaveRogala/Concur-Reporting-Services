using ConcurExpense.Models;
using ConcurReporting.Helpers;
using ConcurReportingDatabaseServices.Models;

namespace ConcurReporting.Mappers;

internal static class AllocationMapper
{
    public static Allocation AllocationFromDto(this AllocationDto dto, DateTime utcNow)
    {
        return new Allocation
        {
            CompanyCode = dto.Custom1?.Code,
            CostCenterCode = dto.Custom2?.Code,
            Department = dto.Custom3?.Code,
            ConcurID = dto.ID,
            Account = dto.AccountNumber ?? "",
            Account1 = dto.AccountCode2,
            IsHidden = dto.IsHidden,
            IsPercentEdited = dto.IsPercentEdited,
            Percentage = dto.Percentage.ToDecimal() ?? 0,
            ProjectCode = dto.Custom6?.Code,
            ProjectDescription = dto.Custom6?.Value,
            DateTimeAddedUtc = utcNow,
            DateTimeUpdatedUtc = utcNow
        };
    }
    public static Allocation UpdateAllocationFromDto(this Allocation entity, AllocationDto dto, DateTime utcNow)
    {
        entity.CompanyCode = dto.Custom1?.Code;
        entity.CostCenterCode = dto.Custom2?.Code;
        entity.Department = dto.Custom3?.Code;
        entity.ConcurID = dto.ID;
        entity.Account = dto.AccountNumber ?? "";
        entity.Account1 = dto.AccountCode2;
        entity.IsHidden = dto.IsHidden;
        entity.IsPercentEdited = dto.IsPercentEdited;
        entity.Percentage = dto.Percentage.ToDecimal() ?? 0;
        entity.ProjectCode = dto.Custom6?.Code;
        entity.ProjectDescription = dto.Custom6?.Value;
        entity.DateTimeUpdatedUtc = utcNow;

        return entity;
    }

}
