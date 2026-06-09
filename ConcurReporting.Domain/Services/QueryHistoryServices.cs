using ConcurReporting.Domain.Models;
using ConcurReporting.Domain.Services.Interfaces;
using ConcurReportingDatabaseServices.Data;
using GenericRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConcurReporting.Domain.Services;

internal class QueryHistoryServices : IQueryHistoryServices
{
    private bool disposedValue;
    private readonly IGenericRepository<QueryHistory, ConcurContext, int> _repository;
    private readonly ILogger<QueryHistoryServices> _logger;
    public QueryHistoryServices(IGenericRepository<QueryHistory, ConcurContext, int> repository
                               ,ILogger<QueryHistoryServices> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<QueryHistory> AddQueryHistoryAsync(QueryHistory queryHistory)
    {
        try
        {
            return await _repository.AddAsync(queryHistory);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Add Query History failed.  Message: {msg}",ex.Message);
            throw;
        }
    }

    public async Task<QueryHistory?> GetLastQueryHistoryAsync(bool IsSuccess = true)
    {
        try
        {            
            return await _repository.FindFirstAsync(q => q.IsSuccess == IsSuccess, e => e.OrderByDescending(o => o.DateTimeAddedUtc),QueryTrackingBehavior.NoTracking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get Last Query History failed.  Message: {msg}", ex.Message);
            throw;
        }
    }

    public async Task<List<QueryHistory>> GetQueryHistoriesAsync(DateTime? dateTimeUpdatedUtcFrom = null, bool IsSuccess = true)
    {
        try
        {
            return (dateTimeUpdatedUtcFrom is null ? await _repository.FindAsync(q => q.IsSuccess == IsSuccess)
                                                   : await _repository.FindAsync(q => q.IsSuccess && 
                                                                                      q.DateTimeAddedUtc >= dateTimeUpdatedUtcFrom)).ToList();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get Query History failed.  Message: {msg}", ex.Message);
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                _repository.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~QueryHistoryServices()
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

    public async Task<int> SaveChangesAsync()
    {
        return await _repository.SaveChangesAsync();
    }
}
