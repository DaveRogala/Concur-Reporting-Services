using ConcurReportingDatabaseServices.Data.Configurations.Bases;
using ConcurReportingDatabaseServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurReportingDatabaseServices.Data.Configurations;

internal class ItemizationConfig : EntryItemizationBaseConfig<Itemization>
{
    public override void Configure(EntityTypeBuilder<Itemization> entity)
    {
        base.Configure(entity);

        entity.ToTable("Itemizations", schema: "cnc");
        entity.Property(e => e.Id).HasColumnName("ItemizationId");

        entity.HasMany(e => e.Allocations)
            .WithOne(e => e.Itemization)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
