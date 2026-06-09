# ConcurReportingServices.Tests

xUnit test project for the `ConcurReportingServices` package.

## Running the tests

```bash
# Standard run
dotnet test ConcurReportingServices.Tests

# With JUnit XML output for CI artefacts
dotnet test ConcurReportingServices.Tests \
  --logger "junit;LogFilePath=TestResults/test-results.xml"

# Using the shared runsettings file (outputs to ./TestResults/test-results.xml)
dotnet test --settings tests.runsettings
```

The JUnit XML file is produced by [JunitXml.TestLogger](https://github.com/spekt/junit.testlogger) and can be consumed by GitLab CI, GitHub Actions, Jenkins, and most other CI systems.

## Coverage

### `HelpersTests`

Tests the string-extension helper methods in `ConcurReporting.Helpers.Helpers`:

| Method | Cases covered |
|---|---|
| `ToDateOnly` | Valid `yyyy-MM-dd` date, wrong day/month order, slash separator, missing zero-pad, non-date string |
| `ToDateTime` | Valid `yyyy-MM-ddTHH:mm:ss` datetime, date-only string, space separator, non-date string |
| `ToBoolean` | `"Y"` → `true`, `"N"` → `false`, unrecognised value → `null` (case-sensitive) |
| `ToDecimal` | Valid decimal string, invalid string |
| `ToInt` | Valid integer string, invalid string, decimal string |
