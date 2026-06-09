namespace ConcurReportingDatabaseServices.Models.Bases;

public abstract class ConcurEntityBase
{
    public int Id { get; set; }
    public required DateTime DateTimeAddedUtc { get; set; }
    public required DateTime DateTimeUpdatedUtc { get; set; }
}
