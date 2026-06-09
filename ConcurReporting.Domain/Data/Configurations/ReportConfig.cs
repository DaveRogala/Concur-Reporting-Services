using ConcurReportingDatabaseServices.Data.Configurations.Bases;
using ConcurReportingDatabaseServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurReportingDatabaseServices.Data.Configurations;

internal class ReportConfig : ObjectBaseConfig<Report>
{
    public override void Configure(EntityTypeBuilder<Report> entity)
    {
        base.Configure(entity);

        entity.ToTable("Reports", schema: "cnc");
        entity.Property(e => e.Id).HasColumnName("ReportId");

        entity.Property(e => e.AmountDueCompanyCard).HasPrecision(18, 4);
        entity.Property(e => e.AmountDueEmployee).HasPrecision(18, 4);
        entity.Property(e => e.PersonalAmount).HasPrecision(18, 4);
        entity.Property(e => e.Total).HasPrecision(18, 4);
        entity.Property(e => e.TotalApprovedAmount).HasPrecision(18, 4);
        entity.Property(e => e.TotalClaimedAmount).HasPrecision(18, 4);
        entity.Property(e => e.CurrencyCode).HasMaxLength(5);
        entity.Property(e => e.Name).HasMaxLength(200);
        entity.Property(e => e.ApprovalStatusCode).HasMaxLength(10);
        entity.Property(e => e.ApprovalStatusName).HasMaxLength(50);
        entity.Property(e => e.ApproverLoginID).HasMaxLength(50);
        entity.Property(e => e.ApproverName).HasMaxLength(200);
        entity.Property(e => e.CountryISO2).HasMaxLength(2);
        entity.Property(e => e.CountrySubdivision).HasMaxLength(10);
        entity.Property(e => e.EmployeeGroup).HasMaxLength(200);
        entity.Property(e => e.LedgerName).HasMaxLength(20);
        entity.Property(e => e.OwnerLoginID).HasMaxLength(50);
        entity.Property(e => e.OwnerName).HasMaxLength(200);
        entity.Property(e => e.PaymentStatusCode).HasMaxLength(10);
        entity.Property(e => e.PaymentStatusName).HasMaxLength(50);
        entity.Property(e => e.PolicyID).HasMaxLength(64);
        entity.Property(e => e.VendorID).HasMaxLength(200);

        entity.HasIndex(e => e.ApprovalStatusCode).HasFillFactor(90);
        entity.HasIndex(e => e.PaymentStatusCode).HasFillFactor(90);
        entity.HasIndex(e => e.ApproverLoginID).HasFillFactor(90);

        entity.HasMany(e => e.Entries)
            .WithOne(e => e.Report)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
