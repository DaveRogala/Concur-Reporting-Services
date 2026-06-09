using ConcurExpense;
using ConcurReporting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConcurReportingServices;

public static class ServiceCollectionsExtension
{
    /// <summary>
    /// Registers the Concur Expense client and all required services.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="apiConfig">API Delegate to configure <see cref="ConcurOptions"/>.</param>
    /// <param name="dbConfig">Database delegate to configure <see cref="DbContextOptionsBuilder"/></param>
    public static IServiceCollection AddConcurReportingClient(
        this IServiceCollection services,
        Action<ConcurOptions> apiConfig,
        Action<DbContextOptionsBuilder> dbConfig)
    {

        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(apiConfig);
        ArgumentNullException.ThrowIfNull(dbConfig);

        services.AddConcurExpenseClient(apiConfig);
        services.AddConcurReportingDatabaseServices(dbConfig);

        services.AddScoped<IConcurReportClient, ConcurReportClient> ();

        return services;
    }
}
