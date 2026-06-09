# ConcurReportingServices

A .NET 10 library that pulls expense report data from the SAP Concur Expense API and syncs it to a SQL Server database, maintaining an incremental sync history so each run picks up only what changed.

## Installation

```
dotnet add package ConcurReportingServices
```

This package depends on [`ConcurReporting.Domain`](../ConcurReporting.Domain/README.md) for database persistence and the `ConcurExpense` package for the Concur API client. Both are pulled in automatically.

## Quick start

Register all services in one call:

```csharp
builder.Services.AddConcurReportingClient(
    apiConfig: options =>
    {
        options.BaseUrl   = "https://us.api.concursolutions.com";
        options.ClientId  = "...";
        options.ClientSecret = "...";
        options.Username  = "...";
        options.Password  = "...";
    },
    dbConfig: options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Concur"))
);
```

Then inject `IConcurReportClient` where you need to run a sync:

```csharp
public class SyncJob(IConcurReportClient client)
{
    public async Task RunAsync()
    {
        // Incremental sync: resumes from the last successful run.
        // Falls back to 2022-01-01 if no history exists.
        int updated = await client.ProcessEntities();
    }
}
```

## `IConcurReportClient`

| Method | Description |
|--------|-------------|
| `ProcessEntities(DateTime? defaultStartDate = null)` | Incremental sync. Reads the last successful `QueryHistory` record and pages forward in 7-day windows until now. `defaultStartDate` is used only when no history exists. |
| `ProcessEntities(DateTime startDate, DateTime? endDate = null)` | Sync a specific date window. Useful for backfills or re-processing a range. |

Both overloads return the number of database rows affected.

## Object hierarchy

```
Report
└── Entry  (one or more)
    ├── Allocation  (when the entry has no itemizations)
    └── Itemization  (when the entry is itemized)
        └── Allocation
```

Allocations are identified by their Concur ID and are linked to either an `Entry` or an `Itemization`, never both. The library handles several Concur edge cases transparently:

- **Entries moving between reports** – detected and reassigned on the next sync.
- **Allocation orphaning** – allocations whose parent entry or itemization no longer exists are deleted by the `CascadeDeleteInterceptor` before they can cause unique-index conflicts on reactivation.
- **Invalid allocation parents** – allocations whose `EntryID` does not match any current entry or itemization in Concur are filtered out and logged rather than saved.

## Sync history

Every successful and failed sync window is recorded in the `cnc.QueryHistories` table. The incremental `ProcessEntities()` overload uses the latest successful entry to determine where to resume. A failed run records the error message so it is visible without reading logs.

## Logging

The library uses `Microsoft.Extensions.Logging`. Structured log events are emitted at `Information` (per page) and `Warning`/`Error` levels. Bind a provider in the consuming application to capture them.
