# ConcurReporting.Domain.Tests

xUnit test project for the `ConcurReporting.Domain` package.

## Running the tests

```bash
# Standard run
dotnet test ConcurReporting.Domain.Tests

# With JUnit XML output for CI artefacts
dotnet test ConcurReporting.Domain.Tests \
  --logger "junit;LogFilePath=TestResults/test-results.xml"

# Using the shared runsettings file (outputs to ./TestResults/test-results.xml)
dotnet test --settings tests.runsettings
```

Tests use the EF Core InMemory provider so no database is required. The `CascadeDeleteInterceptor` and lazy loading proxies are active (both are registered unconditionally in `ConcurContext.OnConfiguring`).

## Coverage

### `CascadeDeleteInterceptorTests`

Integration tests that verify the interceptor's change-tracker manipulation via a real `ConcurContext` backed by an in-memory database.

| Test | What it verifies |
|---|---|
| `HandleOrphanedAllocations_ModifiedWithBothForeignKeysNull_IsDeleted` | An `Allocation` in `Modified` state with both `EntryId` and `ItemizationId` null is deleted by `SaveChangesAsync`, not just updated. |
| `HandleOrphanedAllocations_ModifiedWithEntryFkSet_IsNotDeleted` | An `Allocation` with a valid `EntryId` that is otherwise modified is left untouched. |
| `HandlePreExistingOrphanedAllocations_DeletesOrphanBeforeInsertingSameConcurId` | A pre-existing orphaned allocation (null FKs) in the database is removed before a new allocation with the same `ConcurID` is inserted, preventing a unique-index violation on re-activation. |
| `HandlePreExistingOrphanedAllocations_NoOrphansInDb_InsertsNormally` | When there are no pre-existing orphans, a new allocation is inserted without error. |

### `QueryHistoryServicesTests`

Integration tests that exercise `QueryHistoryServices` end-to-end through `ConcurGenericRepository` against an in-memory database — no mocks.

| Test | What it verifies |
|---|---|
| `GetQueryHistoriesAsync_NoDateFilter_IsSuccessTrue_ReturnsOnlySuccessRecords` | Without a date filter, only `IsSuccess = true` records are returned when `isSuccess: true`. |
| `GetQueryHistoriesAsync_NoDateFilter_IsSuccessFalse_ReturnsOnlyFailureRecords` | Without a date filter, only `IsSuccess = false` records are returned when `isSuccess: false`. |
| `GetQueryHistoriesAsync_WithDateFilter_IsSuccessTrue_ExcludesFailureAndOldRecords` | With a date cutoff, old records and records with the wrong success flag are excluded. |
| `GetQueryHistoriesAsync_WithDateFilter_IsSuccessFalse_ExcludesSuccessAndOldRecords` | **Regression test** for the bug where the date-filter branch hardcoded `q.IsSuccess` instead of `q.IsSuccess == isSuccess`, which would have returned success records when `false` was requested. |
| `GetLastQueryHistoryAsync_IsSuccessTrue_ReturnsLatestSuccessRecord` | Returns the most-recently-added success record, ignoring failure records. |
| `GetLastQueryHistoryAsync_IsSuccessFalse_ReturnsLatestFailureRecord` | Returns the most-recently-added failure record, ignoring success records. |
| `GetLastQueryHistoryAsync_NoMatchingRecords_ReturnsNull` | Returns `null` when no records match the requested success flag. |
