using ConcurReporting.Domain.Models;

namespace ConcurReporting.Domain.Services.Interfaces;

public interface IQueryHistoryServices : IDisposable
{
    Task<QueryHistory?> GetLastQueryHistoryAsync(bool IsSuccess = true);
    Task<List<QueryHistory>> GetQueryHistoriesAsync(DateTime? dateTimeUpdatedUtcFrom = null, bool IsSuccess = true);
    Task<QueryHistory> AddQueryHistoryAsync(QueryHistory queryHistory);
    Task<int> SaveChangesAsync();
}
