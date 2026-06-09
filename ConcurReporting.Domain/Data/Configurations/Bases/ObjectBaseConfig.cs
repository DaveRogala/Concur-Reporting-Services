using ConcurReportingDatabaseServices.Models.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurReportingDatabaseServices.Data.Configurations.Bases;

internal class ObjectBaseConfig<T> : ConcurEntityBaseConfig<T>
    where T : ObjectBase
{
    
    public override void Configure(EntityTypeBuilder<T> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.Department).HasMaxLength(200);
        entity.Property(e => e.CompanyCode).HasMaxLength(10);
        entity.Property(e => e.CostCenterCode).HasMaxLength(10);

        entity.HasIndex(e => new { e.CompanyCode, e.CostCenterCode })
           .HasFillFactor(90);

        entity.HasIndex(e => e.ConcurID)
            .IsUnique()
            .HasFillFactor(90);
    }
}
