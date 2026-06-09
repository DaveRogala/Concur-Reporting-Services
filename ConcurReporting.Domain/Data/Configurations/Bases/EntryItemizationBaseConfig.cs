using ConcurReportingDatabaseServices.Models.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurReportingDatabaseServices.Data.Configurations.Bases;

internal class EntryItemizationBaseConfig<T> : ObjectBaseConfig<T>
    where T : EntryItemizationBase
{
    public override void Configure(EntityTypeBuilder<T> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.Account).HasMaxLength(200);        
        entity.Property(e => e.AllocationType).HasMaxLength(10);
        entity.Property(e => e.ApprovedAmount).HasPrecision(18, 8);
        entity.Property(e => e.ContractOrRFPTitleAndIdentifyingNumber).HasMaxLength(50);
        entity.Property(e => e.ContractTitleIdentifyingNumberAndDate).HasMaxLength(200);
        entity.Property(e => e.Description).HasMaxLength(64);
        entity.Property(e => e.EntrySubtractFromGross).HasPrecision(18, 4);
        entity.Property(e => e.ExpenseTypeCode).HasMaxLength(200);
        entity.Property(e => e.ExpenseTypeName).HasMaxLength(200);
        entity.Property(e => e.ItemsGiven).HasMaxLength(200);
        entity.Property(e => e.LocationCountryISO2).HasMaxLength(2);
        entity.Property(e => e.LocationName).HasMaxLength(200);
        entity.Property(e => e.LocationSubdivision).HasMaxLength(200);
        entity.Property(e => e.NatureOfContact).HasMaxLength(200);
        entity.Property(e => e.NetTaxAmount).HasPrecision(18, 4);
        entity.Property(e => e.NonTaxableNonDeductibleAmount).HasMaxLength(200);
        entity.Property(e => e.NonTaxDeductibleVATAmount).HasMaxLength(200);
        entity.Property(e => e.PostedAmount).HasPrecision(18, 4);
        entity.Property(e => e.ProjectCode).HasMaxLength(200);
        entity.Property(e => e.ProjectDescription).HasMaxLength(200);
        entity.Property(e => e.ProposedService).HasMaxLength(200);
        entity.Property(e => e.BillRuleRegOrRateNumber).HasMaxLength(200);
        entity.Property(e => e.SpendCategoryCode).HasMaxLength(200);
        entity.Property(e => e.SpendCategoryName).HasMaxLength(200);
        entity.Property(e => e.TaxableDeductibleAmount).HasMaxLength(200);
        entity.Property(e => e.TaxDeductibleVATAmount).HasMaxLength(200);
        entity.Property(e => e.TaxInvoiceNumber).HasMaxLength(200);
        entity.Property(e => e.TaxReclaimAmount).HasPrecision(18, 4);
        entity.Property(e => e.TaxReclaimCountryRegion).HasMaxLength(200);
        entity.Property(e => e.TransactionAmount).HasPrecision(18, 4);
        entity.Property(e => e.VATCode).HasMaxLength(200);

       
    }
}
