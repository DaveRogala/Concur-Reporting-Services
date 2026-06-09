using ConcurReportingDatabaseServices.Models.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConcurReportingDatabaseServices.Data.Configurations.Bases;

internal class ConcurEntityBaseConfig<T> : IEntityTypeConfiguration<T>
    where T : ConcurEntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> entity)
    {
        entity.HasKey(e => e.Id);
    }
}
