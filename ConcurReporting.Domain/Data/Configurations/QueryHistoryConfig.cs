using ConcurReporting.Domain.Models;
using ConcurReportingDatabaseServices.Data.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurReporting.Domain.Data.Configurations;

internal class QueryHistoryConfig : ConcurEntityBaseConfig<QueryHistory>
{
    public override void Configure(EntityTypeBuilder<QueryHistory> entity)
    {
        base.Configure(entity);
        entity.ToTable("QueryHistories", schema: "cnc");

        entity.Property(e => e.Id).HasColumnName("QueryHistoryId");

        entity.HasIndex(e => e.DateTimeAddedUtc);
    }
}
