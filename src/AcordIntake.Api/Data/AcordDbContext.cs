// connection to the database
using AcordIntake.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AcordIntake.Api.Data;

public class AcordDbContext(DbContextOptions<AcordDbContext> options) : DbContext(options)
{
    public DbSet<APSIncomingEntity> APSIncomingEntities => Set<APSIncomingEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<APSIncomingEntity>(entity =>
        {
            entity.ToTable("aps_incoming");
            entity.Property(item => item.PolicyAmcount).HasPrecision(18, 2);
        });
    }
}