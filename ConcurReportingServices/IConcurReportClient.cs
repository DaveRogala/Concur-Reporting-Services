namespace ConcurReporting;

public interface IConcurReportClient : IDisposable
{
    Task<int> ProcessEntities(DateTime startDate, DateTime? endDate = null);
    /// <summary>
    /// Processes all entities from the last successful query up to now, paging in 7-day windows.
    /// Falls back to <paramref name="defaultStartDate"/> when no query history exists.
    /// If no start date is provided defaults to 2022-01-01.
    /// </summary>
    Task<int> ProcessEntities(DateTime? defaultStartDate = null);
}
