using ConcurReportingDatabaseServices.Models.Bases;

namespace ConcurReportingDatabaseServices.Models;

public class Entry : EntryItemizationBase
{
    public string? CompanyCardTransactionID { get; set; } 
    public decimal ExchangeRate { get; set; }
    public string? ExpenseID { get; set; }
    public bool HasAppliedCashAdvance { get; set; }
    public bool HasAttendees { get; set; }
    public bool HasImage { get; set; }
    public bool HasVAT { get; set; }
    public bool IsPaidByExpensePay { get; set; }
    public bool IsPersonalCardCharge { get; set; }
    public string? PaymentTypeName { get; set; }
    public bool ReceiptReceived { get; set; }
    public string? TaxReceiptType { get; set; }
    public required string TransactionCurrencyCode { get; set; }
    public string? TripID { get; set; }
    public string? VendorDescription { get; set; }
    public string? VendorListItemName { get; set; }

    public virtual Journey? Journey { get; set; }
    public virtual List<Itemization>? Itemizations { get; set; }

    public virtual Report Report { get; set; }
}
