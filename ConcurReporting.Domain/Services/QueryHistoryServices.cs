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

    public QueryHistoryServices(IGenericRepository<QueryHistory, ConcurContext, int> repository,
                                ILogger<QueryHistoryServices> logger)
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
            _logger.LogError(ex, "AddQueryHistoryAsync failed. Message: {msg}", ex.Message);
            throw;
        }
    }

    public async Task<QueryHistory?> GetLastQueryHistoryAsync(bool isSuccess = true)
    {
        try
        {
            return await _repository.FindFirstAsync(q => q.IsSuccess == isSuccess, e => e.OrderByDescending(o => o.DateTimeAddedUtc), QueryTrackingBehavior.NoTracking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetLastQueryHistoryAsync failed. Message: {msg}", ex.Message);
            throw;
        }
    }

    public async Task<List<QueryHistory>> GetQueryHistoriesAsync(DateTime? dateTimeUpdatedUtcFrom = null, bool isSuccess = true)
    {
        try
        {
            return (dateTimeUpdatedUtcFrom is null ? await _repository.FindAsync(q => q.IsSuccess == isSuccess)
                                                   : await _repository.FindAsync(q => q.IsSuccess == isSuccess &&
                                                                                      q.DateTimeAddedUtc >= dateTimeUpdatedUtcFrom)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetQueryHistoriesAsync failed. Message: {msg}", ex.Message);
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                _repository.Dispose();

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _repository.SaveChangesAsync();
    }
}
