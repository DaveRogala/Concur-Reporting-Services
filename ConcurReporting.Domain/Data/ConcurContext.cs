using ConcurReporting.Domain.Models;
using ConcurReportingDatabaseServices.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcurReportingDatabaseServices.Data;

internal partial class ConcurContext : DbContext
{
    public ConcurContext(DbContextOptions options) 
        : base(options)
    {
    }
    public ConcurContext()
    {
    }
    public virtual DbSet<Report> Reports { get; set; }
    public virtual DbSet<Entry> Entries { get; set; }
    public virtual DbSet<Itemization> Itemizations { get; set; }
    public virtual DbSet<Journey> Journeys { get; set; }
    public virtual DbSet<Allocation> Allocations { get; set; }
    public virtual DbSet<QueryHistory> QueryHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new CascadeDeleteInterceptor())
            .UseSqlServer()
            .UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
