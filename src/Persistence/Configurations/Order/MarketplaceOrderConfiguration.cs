using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Order.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Order;

internal class MarketplaceOrderEventConfiguration: IEntityTypeConfiguration<MarketplaceOrder>
{
    public void Configure(EntityTypeBuilder<MarketplaceOrder> b)
    {
        b.ToTable("marketplaceOrders");
        b.Property(e => e.Id).ValueGeneratedNever();
        
        b.Property(e => e.Status).HasConversion(toDb => toDb.Value, fromDb => OrderStatus.FromValue(fromDb)).HasMaxLength(50);
        
        b.HasIndex(e => new{ e.MarketplaceOrderId, e.ShippingContractId }).IsUnique();
        
        b.Property(e => e.MarketplaceOrderId).HasMaxLength(50);
        b.HasOne(e => e.ShippingContract).WithMany(e => e.Orders).HasForeignKey(e => e.ShippingContractId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.Events).WithOne(e => e.Order).HasForeignKey(e => e.OrderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}