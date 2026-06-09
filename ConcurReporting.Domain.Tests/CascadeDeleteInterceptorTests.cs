using ConcurReportingDatabaseServices.Data;
using ConcurReportingDatabaseServices.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcurReporting.Domain.Tests;

public class CascadeDeleteInterceptorTests
{
    private static readonly DateTime UtcNow = new(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc);

    private static ConcurContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ConcurContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ConcurContext(options);
    }

    private static Allocation NewAllocation(string concurId, string account = "4000") =>
        new()
        {
            ConcurID = concurId,
            Account = account,
            IsHidden = false,
            IsPercentEdited = false,
            Percentage = 100m,
            DateTimeAddedUtc = UtcNow,
            DateTimeUpdatedUtc = UtcNow,
        };

    // ── HandleOrphanedAllocations ────────────────────────────────────────────

    [Fact]
    public async Task HandleOrphanedAllocations_ModifiedWithBothForeignKeysNull_IsDeleted()
    {
        // Arrange: save an allocation with no parent (both FKs null) so it is Unchanged.
        await using var context = CreateContext("orphan-modified-1");
        var alloc = NewAllocation("ALLOC-001");
        context.Allocations.Add(alloc);
        await context.SaveChangesAsync(); // interceptor ignores Added state

        // Simulate EF Core ClientSetNull: the entity is Modified but both FKs are null.
        context.Entry(alloc).State = EntityState.Modified;

        // Act
        await context.SaveChangesAsync();

        // Assert: interceptor detected the Modified+null-FK allocation and deleted it.
        Assert.Equal(0, context.Allocations.Count());
    }

    [Fact]
    public async Task HandleOrphanedAllocations_ModifiedWithEntryFkSet_IsNotDeleted()
    {
        // Arrange: create a Report → Entry → Allocation hierarchy so EntryId is non-null.
        await using var context = CreateContext("orphan-modified-2");

        var report = new Report
        {
            ConcurID = "RPT-001",
            Name = "Test",
            CurrencyCode = "USD",
            LastModifiedDateTimeUtc = UtcNow,
            DateTimeAddedUtc = UtcNow,
            DateTimeUpdatedUtc = UtcNow,
        };
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        var entry = new Entry
        {
            ConcurID = "ENT-001",
            ExpenseTypeCode = "TRAVEL",
            TransactionCurrencyCode = "USD",
            TransactionAmount = 50m,
            LastModifiedDateTimeUtc = UtcNow,
            DateTimeAddedUtc = UtcNow,
            DateTimeUpdatedUtc = UtcNow,
        };
        report.Entries.Add(entry);
        await context.SaveChangesAsync();

        var alloc = NewAllocation("ALLOC-001");
        entry.Allocations.Add(alloc);
        await context.SaveChangesAsync();

        // Modify the allocation (without nulling the FK).
        alloc.Account = "5000";
        context.Entry(alloc).State = EntityState.Modified;

        // Act
        await context.SaveChangesAsync();

        // Assert: allocation with a valid EntryId is NOT deleted.
        Assert.Equal(1, context.Allocations.Count());
    }

    // ── HandlePreExistingOrphanedAllocations ─────────────────────────────────

    [Fact]
    public async Task HandlePreExistingOrphanedAllocations_DeletesOrphanBeforeInsertWithSameConcurId()
    {
        // Arrange: first save – an orphan lands in the DB (both FKs null).
        await using var context = CreateContext("preexisting-orphan-1");

        var orphan = NewAllocation("ALLOC-001", "OLD-ACCOUNT");
        context.Allocations.Add(orphan);
        await context.SaveChangesAsync();

        // Detach so a later query treats it as a fresh DB row.
        context.Entry(orphan).State = EntityState.Detached;

        // Second save – a new allocation arrives with the same ConcurID.
        var incoming = NewAllocation("ALLOC-001", "NEW-ACCOUNT");
        context.Allocations.Add(incoming);

        // Act: the interceptor should delete the orphan before inserting the incoming row.
        await context.SaveChangesAsync();

        // Assert: only one allocation exists and it is the newly inserted one.
        var allocs = context.Allocations.AsNoTracking().ToList();
        Assert.Single(allocs);
        Assert.Equal("NEW-ACCOUNT", allocs[0].Account);
    }

    [Fact]
    public async Task HandlePreExistingOrphanedAllocations_NoOrphansInDb_InsertsNormally()
    {
        // Arrange: no pre-existing orphan in DB.
        await using var context = CreateContext("preexisting-orphan-2");

        var alloc = NewAllocation("ALLOC-001", "FIRST-ACCOUNT");
        context.Allocations.Add(alloc);

        // Act
        await context.SaveChangesAsync();

        // Assert: the single allocation was inserted.
        Assert.Equal(1, context.Allocations.Count());
        Assert.Equal("FIRST-ACCOUNT", context.Allocations.First().Account);
    }
}
