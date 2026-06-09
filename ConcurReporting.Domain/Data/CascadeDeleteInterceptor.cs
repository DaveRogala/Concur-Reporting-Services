using ConcurReportingDatabaseServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public sealed class CascadeDeleteInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        if (context is null)
            return result;

        HandleEntryLevelDeletes(context);
        HandleOrphanedAllocations(context);
        HandlePreExistingOrphanedAllocations(context).GetAwaiter().GetResult();

        return result;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
            return result;

        HandleEntryLevelDeletes(context);
        HandleOrphanedAllocations(context);
        await HandlePreExistingOrphanedAllocations(context, cancellationToken);

        return result;
    }

    private static void HandleEntryLevelDeletes(DbContext context)
    {
        // Ensure EF scans navigation properties
        context.ChangeTracker.DetectChanges();

        // Find all Entry entities marked for deletion
        var deletedEntries = context.ChangeTracker.Entries<Entry>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity)
            .ToList();

        if (deletedEntries.Count == 0)
            return;

        foreach (var entry in deletedEntries)
        {
            // Ensure navigations are loaded for delete work
            context.Entry(entry).Collection(e => e.Allocations).Load();
            context.Entry(entry).Collection(e => e.Itemizations).Query()
                .Include(i => i.Allocations)
                .Load();

            // 1. Delete allocations directly belonging to the Entry
            if (entry.Allocations?.Count > 0)
                context.RemoveRange(entry.Allocations);

            // 2. Delete itemizations (their allocations cascade in DB)
            if (entry.Itemizations?.Count > 0)
                context.RemoveRange(entry.Itemizations);
        }
    }

    private static void HandleOrphanedAllocations(DbContext context)
    {
        // Allocation.EntryId and Allocation.ItemizationId are optional (nullable) shadow FKs.
        // When an allocation is removed from a tracked collection EF Core uses ClientSetNull —
        // it nulls both FKs rather than deleting the row, producing an orphaned record.
        // Detect any allocation in Modified state where both FKs are null and delete it instead.
        context.ChangeTracker.DetectChanges();

        var orphaned = context.ChangeTracker.Entries<Allocation>()
            .Where(e => e.State == EntityState.Modified
                        && e.Property("EntryId").CurrentValue is null
                        && e.Property("ItemizationId").CurrentValue is null)
            .ToList();

        foreach (var entry in orphaned)
            entry.State = EntityState.Deleted;
    }

    private static async Task HandlePreExistingOrphanedAllocations(
        DbContext context,
        CancellationToken cancellationToken = default)
    {
        // If Concur reactivates an allocation whose ConcurID already exists in the DB as an orphan
        // (EntryId and ItemizationId both null from a previous sync), the INSERT for the incoming
        // allocation would fail on the unique index. Find and delete any such pre-existing orphans
        // before the new rows are inserted so the save can proceed cleanly.
        var incomingConcurIds = context.ChangeTracker.Entries<Allocation>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity.ConcurID)
            .ToHashSet();

        if (incomingConcurIds.Count == 0)
            return;

        var preExistingOrphans = await context.Set<Allocation>()
            .Where(a => incomingConcurIds.Contains(a.ConcurID)
                        && EF.Property<int?>(a, "EntryId") == null
                        && EF.Property<int?>(a, "ItemizationId") == null)
            .ToListAsync(cancellationToken);

        if (preExistingOrphans.Count > 0)
            context.RemoveRange(preExistingOrphans);
    }
}