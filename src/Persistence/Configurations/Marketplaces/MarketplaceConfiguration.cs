using Domain;
using Domain.Marketplaces;
using Domain.Order.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Marketplaces;

internal class MarketplaceConfiguration: IEntityTypeConfiguration<Marketplace>
{
    public void Configure(EntityTypeBuilder<Marketplace> b)
    {
        b.ToTable("marketplaces");
        b.Property(e => e.Id).ValueGeneratedNever();
        
        b.Property(e => e.TradeLicenseNumber).HasMaxLength(30);
        b.Property(e => e.NameEn).HasMaxLength(30);
        b.Property(e => e.NameAr).HasMaxLength(30);
        b.Property(e => e.Address).HasMaxLength(50);
        b.Property(e => e.WebsiteLink).HasMaxLength(200);
        b.Property(e => e.Email).HasMaxLength(50);
        b.Property(e => e.PhoneNumber).HasMaxLength(20);
        
        b.HasIndex(e => e.TradeLicenseNumber).IsUnique();
        b.HasIndex(e => e.Email).IsUnique();
        b.HasIndex(e => e.PhoneNumber).IsUnique();
        
        b.HasMany(e => e.Contracts).WithOne(e => e.Marketplace).IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}