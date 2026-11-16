using Domain;
using Domain.Marketplaces;
using Domain.Order;
using Domain.Shippers;
using Domain.ShippingContracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<MarketplaceOrder> Orders { get; set; }
    public DbSet<OrderEvent> OrderEvents { get; set; }
    public DbSet<Marketplace> Marketplaces { get; set; }
    public DbSet<Shipper> Shippers { get; set; }
    public DbSet<ShippingContract> ShippingContracts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyBaseEntityInfo();
        ApplyAuditInfo();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                entry.Entity.LastModifiedOn =  DateTime.UtcNow;
        }
    }

    private void ApplyBaseEntityInfo()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedOn =  DateTime.UtcNow;
        }
    }
}