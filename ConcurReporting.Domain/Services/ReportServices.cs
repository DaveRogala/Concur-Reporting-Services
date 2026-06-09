using ConcurReportingDatabaseServices.Data;
using ConcurReportingDatabaseServices.Models;
using ConcurReportingDatabaseServices.Services.Interfaces;
using GenericRepositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ConcurReportingDatabaseServices.Services;

internal class ReportServices : IReportServices
{
    private bool disposedValue;
    private readonly IGenericRepository<Report, ConcurContext, int> _reportRepository;
    private readonly ILogger<ReportServices> _logger;

    public ReportServices(IGenericRepository<Report, ConcurContext, int> reportRepository,
        ILogger<ReportServices> logger)
    {
        _logger = logger;
        _reportRepository = reportRepository;
    }

    public async Task<List<Report>> FindReportsAsync(Expression<Func<Report, bool>> predicate)
    {
        try
        {
            return (await _reportRepository.FindAsync(predicate)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FindReportAsync failed. Message: {msg}", ex.Message);
            throw;
        }
    }

    public async Task<Report?> GetReportAsync(int id)
    {
        try
        {
            return await _reportRepository.GetAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetReportAsync failed. Message: {msg}", ex.Message);
            throw;
        }
    }

    public Report UpdateReport(Report report)
    {
        try
        {
            return _reportRepository.Update(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateReport failed. Message: {msg}", ex.Message);
            throw;
        }
    }

    public async Task<Report> AddReportAsync(Report report)
    {
        try
        {
            return await _reportRepository.AddAsync(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddReportAsync failed. Message: {msg}", ex.Message);
            throw;
        }
    }

    public async Task<List<Report>> FindReportsAsync(List<string> concurIds)
    {
        return await FindReportsAsync(r => concurIds.Contains(r.ConcurID));
    }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _reportRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SaveChangesAsync failed. Message: {msg}", ex.Message);
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                _reportRepository.Dispose();

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
