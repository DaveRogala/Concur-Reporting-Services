using ConcurReporting.Domain.Models;
using ConcurReporting.Domain.Services;
using ConcurReporting.Domain.Services.Interfaces;
using ConcurReportingDatabaseServices.Data;
using ConcurReportingDatabaseServices.Models;
using GenericRepositories.Interfaces;
using GenericRepositories;
using ConcurReportingDatabaseServices.Services;
using ConcurReportingDatabaseServices.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ConcurReporting.Domain;

public static class ServiceCollectionExtensions
{
    ///<summary>
    ///Registers the Concur Reporting Database Services
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="options">DB Context Options <see cref="DbContextOptionsBuilder"/></param>

    public static IServiceCollection AddConcurReportingDatabaseServices(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> options)
    {
        
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);

        services.AddDbContextFactory<ConcurContext>(options);

        services.AddScoped<IGenericRepository<Report,ConcurContext,int>, ConcurGenericRepository<Report>>();
        services.AddScoped<IGenericRepository<Allocation, ConcurContext, int>, ConcurGenericRepository<Allocation>>();
        services.AddScoped<IGenericRepository<QueryHistory, ConcurContext, int>, ConcurGenericRepository<QueryHistory>>();

        services.AddScoped<IReportServices,ReportServices>();
        services.AddScoped<IQueryHistoryServices,QueryHistoryServices>();
        
        return services;

    }
}