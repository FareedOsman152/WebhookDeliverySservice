using Domain;
using Domain.Order.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Order;

internal class MarketplaceOrderConfiguration: IEntityTypeConfiguration<OrderEvent>
{
    public void Configure(EntityTypeBuilder<OrderEvent> b)
    {
        b.ToTable("marketplaceOrderEvents");
        b.Property(e => e.Id).ValueGeneratedNever();
        
        b.Property(e => e.EventType).HasConversion(toDb => toDb.Value, fromDb => OrderEventType.FromValue(fromDb)).HasMaxLength(50);
        b.Property(e => e.StatusBefore).HasConversion(toDb => toDb == null ? null : toDb.Value, fromDb => fromDb == null ? null : OrderStatus.FromValue(fromDb)).HasMaxLength(50);
        b.Property(e => e.StatusAfter).HasConversion(toDb => toDb.Value, fromDb => OrderStatus.FromValue(fromDb)).HasMaxLength(50);
    }
}