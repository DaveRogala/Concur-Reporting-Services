using ConcurReporting.Domain.Models;

namespace ConcurReporting.Domain.Services.Interfaces;

public interface IQueryHistoryServices : IDisposable
{
    Task<QueryHistory?> GetLastQueryHistoryAsync(bool isSuccess = true);
    Task<List<QueryHistory>> GetQueryHistoriesAsync(DateTime? dateTimeUpdatedUtcFrom = null, bool isSuccess = true);
    Task<QueryHistory> AddQueryHistoryAsync(QueryHistory queryHistory);
    Task<int> SaveChangesAsync();
}
