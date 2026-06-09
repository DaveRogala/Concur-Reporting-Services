namespace ConcurReporting;

public interface IConcurReportClient : IDisposable
{
    Task<int> ProcessEntities(DateTime utcNow,DateTime startDate, DateTime? endDate = null );
    /// <summary>
    /// Processes all entities
    /// If there is no query history present will default with the start date provided
    /// If no start Date is provided will default to 2022-01-01
    /// </summary>
    /// <param name="defaultStartDate"></param>
    /// <returns></returns>
    Task<int> ProcessEntities(DateTime? defaultStartDate);
}
