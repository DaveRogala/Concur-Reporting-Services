using ConcurReporting.Domain;
using ConcurReporting.Domain.Models;
using ConcurReporting.Domain.Services;
using ConcurReporting.Domain.Services.Interfaces;
using ConcurReportingDatabaseServices.Data;
using GenericRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace ConcurReporting.Domain.Tests;

/// <summary>
/// Integration-style tests that exercise QueryHistoryServices against an
/// in-memory database to verify the isSuccess filter is applied correctly
/// in both query branches.
/// </summary>
public class QueryHistoryServicesTests
{
    private static readonly DateTime Base = new(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc);

    private static (IQueryHistoryServices service, ConcurContext context) CreateServiceAndContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ConcurContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var context = new ConcurContext(options);
        var repoLogger = NullLogger<GenericRepository<QueryHistory, ConcurContext, int>>.Instance;
        var repository = new ConcurGenericRepository<QueryHistory>(context, repoLogger);
        var serviceLogger = NullLogger<QueryHistoryServices>.Instance;
        IQueryHistoryServices service = new QueryHistoryServices(repository, serviceLogger);

        return (service, context);
    }

    private static QueryHistory SuccessRecord(DateTime addedAt) => new()
    {
        IsSuccess = true,
        Message = "ok",
        DateTimeAddedUtc = addedAt,
        DateTimeUpdatedUtc = addedAt,
    };

    private static QueryHistory FailureRecord(DateTime addedAt) => new()
    {
        IsSuccess = false,
        Message = "error",
        DateTimeAddedUtc = addedAt,
        DateTimeUpdatedUtc = addedAt,
    };

    // ── GetQueryHistoriesAsync – no date filter ───────────────────────────────

    [Fact]
    public async Task GetQueryHistoriesAsync_NoDateFilter_IsSuccessTrue_ReturnsOnlySuccessRecords()
    {
        var (service, context) = CreateServiceAndContext("qh-no-date-success");
        await using var _ = context;

        context.QueryHistories.AddRange(SuccessRecord(Base), FailureRecord(Base));
        await context.SaveChangesAsync();

        var results = await service.GetQueryHistoriesAsync(dateTimeUpdatedUtcFrom: null, isSuccess: true);

        Assert.All(results, r => Assert.True(r.IsSuccess));
        Assert.Single(results);
    }

    [Fact]
    public async Task GetQueryHistoriesAsync_NoDateFilter_IsSuccessFalse_ReturnsOnlyFailureRecords()
    {
        var (service, context) = CreateServiceAndContext("qh-no-date-failure");
        await using var _ = context;

        context.QueryHistories.AddRange(SuccessRecord(Base), FailureRecord(Base));
        await context.SaveChangesAsync();

        var results = await service.GetQueryHistoriesAsync(dateTimeUpdatedUtcFrom: null, isSuccess: false);

        Assert.All(results, r => Assert.False(r.IsSuccess));
        Assert.Single(results);
    }

    // ── GetQueryHistoriesAsync – with date filter ────────────────────────────

    [Fact]
    public async Task GetQueryHistoriesAsync_WithDateFilter_IsSuccessTrue_ExcludesFailureAndOldRecords()
    {
        var (service, context) = CreateServiceAndContext("qh-date-success");
        await using var _ = context;

        var cutoff = Base;
        context.QueryHistories.AddRange(
            SuccessRecord(Base.AddDays(1)),   // should be included
            SuccessRecord(Base.AddDays(-1)),  // too old
            FailureRecord(Base.AddDays(1))    // wrong success flag
        );
        await context.SaveChangesAsync();

        var results = await service.GetQueryHistoriesAsync(cutoff, isSuccess: true);

        Assert.Single(results);
        Assert.True(results[0].IsSuccess);
        Assert.True(results[0].DateTimeAddedUtc >= cutoff);
    }

    [Fact]
    public async Task GetQueryHistoriesAsync_WithDateFilter_IsSuccessFalse_ExcludesSuccessAndOldRecords()
    {
        var (service, context) = CreateServiceAndContext("qh-date-failure");
        await using var _ = context;

        var cutoff = Base;
        context.QueryHistories.AddRange(
            SuccessRecord(Base.AddDays(1)),   // wrong success flag – excluded
            FailureRecord(Base.AddDays(-1)),  // too old – excluded
            FailureRecord(Base.AddDays(1))    // should be the only result
        );
        await context.SaveChangesAsync();

        var results = await service.GetQueryHistoriesAsync(cutoff, isSuccess: false);

        Assert.Single(results);
        Assert.False(results[0].IsSuccess);
        Assert.True(results[0].DateTimeAddedUtc >= cutoff);
    }

    // ── GetLastQueryHistoryAsync ──────────────────────────────────────────────

    [Fact]
    public async Task GetLastQueryHistoryAsync_IsSuccessTrue_ReturnsLatestSuccessRecord()
    {
        var (service, context) = CreateServiceAndContext("qh-last-success");
        await using var _ = context;

        context.QueryHistories.AddRange(
            SuccessRecord(Base.AddDays(-2)),
            SuccessRecord(Base.AddDays(-1)),  // this is the latest success
            FailureRecord(Base)               // failure; should not be returned
        );
        await context.SaveChangesAsync();

        var result = await service.GetLastQueryHistoryAsync(isSuccess: true);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(Base.AddDays(-1), result.DateTimeAddedUtc);
    }

    [Fact]
    public async Task GetLastQueryHistoryAsync_IsSuccessFalse_ReturnsLatestFailureRecord()
    {
        var (service, context) = CreateServiceAndContext("qh-last-failure");
        await using var _ = context;

        context.QueryHistories.AddRange(
            FailureRecord(Base.AddDays(-2)),
            FailureRecord(Base.AddDays(-1)), // latest failure
            SuccessRecord(Base)              // success; should not be returned
        );
        await context.SaveChangesAsync();

        var result = await service.GetLastQueryHistoryAsync(isSuccess: false);

        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(Base.AddDays(-1), result.DateTimeAddedUtc);
    }

    [Fact]
    public async Task GetLastQueryHistoryAsync_NoMatchingRecords_ReturnsNull()
    {
        var (service, context) = CreateServiceAndContext("qh-last-empty");
        await using var _ = context;

        context.QueryHistories.Add(SuccessRecord(Base));
        await context.SaveChangesAsync();

        var result = await service.GetLastQueryHistoryAsync(isSuccess: false);

        Assert.Null(result);
    }
}
