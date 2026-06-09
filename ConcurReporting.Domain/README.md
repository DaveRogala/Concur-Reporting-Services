# ConcurReporting.Domain

A .NET 10 EF Core library providing the SQL Server database context, entity models, migrations, and repository services for the Concur Expense reporting database.

## Installation

```
dotnet add package ConcurReporting.Domain
```

## Quick start

Register the database services and supply the connection string:

```csharp
builder.Services.AddConcurReportingDatabaseServices(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Concur"))
);
```

`AddConcurReportingDatabaseServices` registers:

| Registered type | Implementation |
|---|---|
| `IReportServices` | `ReportServices` |
| `IQueryHistoryServices` | `QueryHistoryServices` |
| `IDbContextFactory<ConcurContext>` | EF Core factory (scoped) |

## Running migrations

The package ships EF Core migrations that create the `cnc` schema. Apply them against the target database:

```bash
dotnet ef database update \
  --project ConcurReporting.Domain \
  --startup-project <your-startup-project>
```

All tables live in the `cnc` schema.

## Data model

```
cnc.Reports
└── cnc.Entries          (FK: ReportId, cascade delete)
    ├── cnc.Journeys     (FK: JourneyId on Entry, cascade delete)
    ├── cnc.Allocations  (FK: EntryId nullable, client cascade)
    └── cnc.Itemizations (FK: EntryId required, cascade delete)
        └── cnc.Allocations (FK: ItemizationId nullable, DB cascade)

cnc.QueryHistories       (sync run log)
```

All entity tables have a `ConcurID` column with a unique index used as the natural key when syncing from the API.

## Services

### `IReportServices`

```csharp
Task<Report?> GetReportAsync(int id);
Task<Report>  AddReportAsync(Report report);
Report        UpdateReport(Report report);
Task<List<Report>> FindReportsAsync(Expression<Func<Report, bool>> predicate);
Task<List<Report>> FindReportsAsync(List<string> concurIds);
Task<int>     SaveChangesAsync();
```

### `IQueryHistoryServices`

```csharp
Task<QueryHistory?>      GetLastQueryHistoryAsync(bool isSuccess = true);
Task<List<QueryHistory>> GetQueryHistoriesAsync(DateTime? dateTimeUpdatedUtcFrom = null, bool isSuccess = true);
Task<QueryHistory>       AddQueryHistoryAsync(QueryHistory queryHistory);
Task<int>                SaveChangesAsync();
```

## Allocation orphan handling

Concur's optional FK relationships for `Allocation → Entry` and `Allocation → Itemization` default to EF Core's `ClientSetNull` behaviour, which nulls FK columns rather than deleting rows when an allocation is removed from a collection. The `CascadeDeleteInterceptor` intercepts every `SaveChangesAsync` call and:

1. **Deletes Modified allocations with both FKs null** – catches any allocation that EF Core has just client-null'd.
2. **Deletes pre-existing DB orphans** – before inserting new allocations, queries for existing rows with the same `ConcurID` and null FKs, removing them so the unique-index constraint cannot fire on re-activation.

`EntryConfig` also configures `OnDelete(DeleteBehavior.ClientCascade)` for the `Entry → Allocation` relationship so EF Core cascades allocation deletions on the client side when an entry is explicitly deleted.

## EF Core notes

- **Lazy loading proxies** are enabled and applied regardless of whether the `DbContextOptions` are supplied externally. Navigation properties must remain `virtual`.
- The `CascadeDeleteInterceptor` is always registered in `OnConfiguring` so it is active in every context instance, including those created by the `IDbContextFactory`.
- Migrations include a fill-factor index on `QueryHistory.DateTimeAddedUtc` to support the ordered lookup in `GetLastQueryHistoryAsync`.
