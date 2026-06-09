using ConcurReportingDatabaseServices.Models.Bases;

namespace ConcurReporting.Domain.Models;

public class QueryHistory : ConcurEntityBase
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}
