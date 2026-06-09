using ConcurReportingDatabaseServices.Models.Bases;

namespace ConcurReportingDatabaseServices.Models;

public class Itemization : EntryItemizationBase
{
    public virtual Entry Entry { get; set; }
}
