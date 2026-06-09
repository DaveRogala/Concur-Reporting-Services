using ConcurReportingDatabaseServices.Models.Bases;

namespace ConcurReportingDatabaseServices.Models;

public class Journey : ConcurEntityBase
{
    public string? StartLocation { get; set; }
    public string? EndLocation { get; set; }
    public string? UnitOfMeasure { get; set; }
    public int? OdometerStart { get; set; }
    public int? OdometerEnd { get; set; }
    public int? BusinessDistance { get; set; }
    public int? PersonalDistance { get; set; }
    public int? NumberOfPassengers { get; set; }

    public virtual Entry Entry { get; set; }

}
