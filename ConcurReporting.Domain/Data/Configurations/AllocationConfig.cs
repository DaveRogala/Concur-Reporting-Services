using ConcurReportingDatabaseServices.Data.Configurations.Bases;
using ConcurReportingDatabaseServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurReportingDatabaseServices.Data.Configurations;

internal class AllocationConfig : ObjectBaseConfig<Allocation>
{
    public override void Configure(EntityTypeBuilder<Allocation> entity)
    {
        base.Configure(entity);

        entity.ToTable("Allocations", schema: "cnc");
        entity.Property(e => e.Id).HasColumnName("AllocationId");

        entity.Property(e => e.Percentage).HasPrecision(18, 4);
        entity.Property(e => e.Account).HasMaxLength(48);
        entity.Property(e => e.Account1).HasMaxLength(48);
        entity.Property(e => e.ProjectCode).HasMaxLength(48);
        entity.Property(e => e.ProjectDescription).HasMaxLength(48);

        entity.HasIndex(e => new { e.CompanyCode, e.CostCenterCode, e.Account })
            .HasFillFactor(90);
        
    }
}
