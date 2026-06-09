using ConcurReportingDatabaseServices.Data.Configurations.Bases;
using ConcurReportingDatabaseServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurReportingDatabaseServices.Data.Configurations;

internal class EntryConfig : EntryItemizationBaseConfig<Entry>
{
    public override void Configure(EntityTypeBuilder<Entry> entity)
    {
        base.Configure(entity);

        entity.ToTable("Entries", schema: "cnc");
        entity.Property(e => e.Id).HasColumnName("EntryId");

        entity.Property(e => e.ExchangeRate).HasPrecision(18, 4);
        entity.Property(e => e.ExpenseID).HasMaxLength(50);
        entity.Property(e => e.PaymentTypeName).HasMaxLength(50);
        entity.Property(e => e.TaxReceiptType).HasMaxLength(5);
        entity.Property(e => e.TransactionCurrencyCode).HasMaxLength(5);
        entity.Property(e => e.VendorDescription).HasMaxLength(64);
        entity.Property(e => e.VendorListItemName).HasMaxLength(200);
        entity.Property(e => e.CompanyCardTransactionID).HasMaxLength(50);
        entity.Property(e => e.TripID).HasMaxLength(50);    

        entity.HasMany(e => e.Itemizations)
            .WithOne(e => e.Entry)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(e => e.Allocations)
            .WithOne(e => e.Entry);

        entity.HasOne(e => e.Journey)
            .WithOne(e => e.Entry)
            .HasForeignKey<Entry>("JourneyId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
