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

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
            return ValueTask.FromResult(result);

        HandleEntryLevelDeletes(context);

        return ValueTask.FromResult(result);
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
}