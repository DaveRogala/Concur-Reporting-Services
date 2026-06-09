using ConcurReportingDatabaseServices.Data.Configurations.Bases;
using ConcurReportingDatabaseServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurReportingDatabaseServices.Data.Configurations;

internal class JourneyConfig : ConcurEntityBaseConfig<Journey>
{
    public override void Configure(EntityTypeBuilder<Journey> entity)
    {
        base.Configure(entity);

        entity.ToTable("Journeys", schema: "cnc");
        entity.Property(e => e.Id).HasColumnName("JourneyId");

        entity.Property(e => e.StartLocation).HasMaxLength(100);
        entity.Property(e => e.EndLocation).HasMaxLength(100);

        entity.Property(e => e.UnitOfMeasure).HasMaxLength(5);


    }
}
