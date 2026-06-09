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
            _logger.LogError(ex, "FindReportAsync failed.  Exception: {msg}", ex.Message);
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
            _logger.LogError(ex, "GetReportAsyncFailed. Message: {msg}", ex.Message);
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
            _logger.LogError(ex, "UpdateReportFailed. Message: {msg}", ex.Message);
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
            _logger.LogError(ex, "AddReportAsyncFailed. Message: {msg}", ex.Message);
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
            {
                _reportRepository.Dispose();
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~ReportServices()
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

   
}
