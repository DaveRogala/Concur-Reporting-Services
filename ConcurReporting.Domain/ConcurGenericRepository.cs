using ConcurReportingDatabaseServices.Data;
using GenericRepositories;
using Microsoft.Extensions.Logging;


namespace ConcurReporting.Domain;

internal class ConcurGenericRepository<T>(ConcurContext context, ILogger<GenericRepository<T, ConcurContext, int>> logger)
    : GenericRepository<T, ConcurContext, int>(context, logger)
    where T : class;

