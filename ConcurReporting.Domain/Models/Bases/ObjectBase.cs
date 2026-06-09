namespace ConcurReportingDatabaseServices.Models.Bases;

public abstract class ObjectBase : ConcurEntityBase
{
    
    public string? CompanyCode { get; set; }
    public string? CostCenterCode { get; set; }
    public string? Department { get; set; }
    public required string ConcurID { get; set; }

}
