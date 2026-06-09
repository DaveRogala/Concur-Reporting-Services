using ConcurReportingDatabaseServices.Models.Bases;

namespace ConcurReportingDatabaseServices.Models;

public class Allocation : ObjectBase
{
    public required bool IsHidden { get; set; }
    public required bool IsPercentEdited { get; set; }
    public required decimal Percentage { get; set; }
    public required string Account { get; set; }
    public string? Account1 { get; set; }
    public string? ProjectCode { get; set; }
    public string? ProjectDescription { get; set; }

    public virtual Entry? Entry { get; set; }
    public virtual Itemization? Itemization { get; set; }
}
