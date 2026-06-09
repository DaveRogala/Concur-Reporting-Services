using ConcurReportingDatabaseServices.Models.Bases;

namespace ConcurReportingDatabaseServices.Models;

public class Report : ObjectBase
{    
    public decimal AmountDueCompanyCard { get; set; }
    public decimal AmountDueEmployee { get; set; }
    public string? ApprovalStatusCode { get; set; }
    public string? ApprovalStatusName { get; set; }
    public string? ApproverLoginID { get; set; }
    public string? ApproverName { get; set; }
    public string? CountryISO2 { get; set; }
    public string? CountrySubdivision { get; set; }
    public DateTime? CreateDate { get; set; }
    public required string CurrencyCode { get; set; }
    public string? EmployeeGroup { get; set; }
    public bool EverSentBack { get; set; }
    public bool HasException { get; set; }
    public string? LastComment { get; set; }
    public string? LedgerName { get; set; }
    public required string Name { get; set; }
    public string? OwnerLoginID { get; set; }
    public string? OwnerName { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? PaymentStatusCode { get; set; }
    public string? PaymentStatusName { get; set; }
    public decimal PersonalAmount { get; set; }
    public string? PolicyID { get; set; }
    public DateTime? ProcessingPaymentDate { get; set; }
    public bool ReceiptsReceived { get; set; }
    public DateTime? SubmitDate { get; set; }
    public decimal Total { get; set; }
    public decimal TotalApprovedAmount { get; set; }
    public decimal TotalClaimedAmount { get; set; }
    public DateTime? UserDefinedDate { get; set; }
    public string? VendorID { get; set; }

    public required DateTime LastModifiedDateTimeUtc { get; set; }

    public virtual List<Entry> Entries { get; set; } = [];
}
