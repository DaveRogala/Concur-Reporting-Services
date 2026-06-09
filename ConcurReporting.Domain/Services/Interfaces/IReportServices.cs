using ConcurReportingDatabaseServices.Models;
using System.Linq.Expressions;

namespace ConcurReportingDatabaseServices.Services.Interfaces;

public interface IReportServices : IDisposable
{
    Task<Report?> GetReportAsync(int id);
    Report UpdateReport(Report report);
    Task<Report> AddReportAsync(Report report);


    Task<List<Report>> FindReportsAsync(Expression<Func<Report, bool>> predicate);
    /// <summary>
    /// Finds reports by the concureId
    /// </summary>
    /// <param name="concurIds"></param>
    /// <returns></returns>
    Task<List<Report>> FindReportsAsync(List<string> concurIds);
    Task<int> SaveChangesAsync();

}
