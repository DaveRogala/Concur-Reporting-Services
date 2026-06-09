using ConcurReportingDatabaseServices.Models;
using ConcurReportingDatabaseServices.Models.Bases;

public abstract class EntryItemizationBase : ObjectBase
{
    public string? Account { get; set; }
    public string? AllocationType { get; set; }
    public decimal ApprovedAmount { get; set; }
    public string? BillRuleRegOrRateNumber { get; set; }
    public DateOnly? CheckInDate { get; set; }
    public DateOnly? CheckOutDate { get; set; }
    public string? ContractOrRFPTitleAndIdentifyingNumber { get; set; }
    public string? ContractTitleIdentifyingNumberAndDate { get; set; }
    public string? Description { get; set; }
    public decimal? EntrySubtractFromGross { get; set; }
    public required string ExpenseTypeCode { get; set; }
    public string? ExpenseTypeName { get; set; }
    public bool HasComments { get; set; }
    public bool HasExceptions { get; set; }
    public bool IsBillable { get; set; }
    public bool IsImageRequired { get; set; }
    public bool IsPersonal { get; set; }
    public string? ItemsGiven { get; set; }
    public int? LengthInHours { get; set; }
    public string? LocationCountryISO2 { get; set; }
    public string? LocationName { get; set; }
    public string? LocationSubdivision { get; set; }
    public string? NatureOfContact { get; set; }
    public decimal? NetTaxAmount { get; set; }
    public string? NonTaxableNonDeductibleAmount { get; set; }
    public string? NonTaxDeductibleVATAmount { get; set; }
    public bool? PersonalChargeConfirmation { get; set; }
    public bool? PersonDesignatedInContractOrRFP { get; set; }
    public decimal PostedAmount { get; set; }
    public string? ProjectCode { get; set; }
    public string? ProjectDescription { get; set; }
    public string? ProposedService { get; set; }
    public string? SpendCategoryCode { get; set; }
    public string? SpendCategoryName { get; set; }
    public string? TaxableDeductibleAmount { get; set; }
    public string? TaxDeductibleVATAmount { get; set; }
    public string? TaxInvoiceNumber { get; set; }
    public decimal? TaxReclaimAmount { get; set; }
    public string? TaxReclaimCountryRegion { get; set; }
    public required decimal TransactionAmount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? VATCode { get; set; }

    public required DateTime LastModifiedDateTimeUtc { get; set; }

    public virtual List<Allocation> Allocations { get; set; } = [];
}