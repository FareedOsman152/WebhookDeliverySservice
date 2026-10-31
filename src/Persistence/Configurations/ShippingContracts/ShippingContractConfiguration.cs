using Domain;
using Domain.ShippingContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.ShippingContracts;

public class ShippingContractConfiguration: IEntityTypeConfiguration<ShippingContract>
{
    public void Configure(EntityTypeBuilder<ShippingContract> b)
    {
        b.ToTable("shippingContracts");
        b.Property(e => e.Id).ValueGeneratedNever();
        
        b.Property(e => e.Status).HasConversion(toDb => toDb.Value, frmDb => ShippingContractStatus.FromValue(frmDb)).HasMaxLength(50);  
        
        b.HasOne(e => e.Marketplace).WithMany(e => e.Contracts).HasForeignKey(e => e.MarketplaceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.Shipper).WithMany(e => e.Contracts).HasForeignKey(e => e.ShipperId).IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}