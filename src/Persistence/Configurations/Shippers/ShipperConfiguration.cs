using Domain;
using Domain.Shippers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Shippers;

internal class ShipperConfiguration: IEntityTypeConfiguration<Shipper>
{
    public void Configure(EntityTypeBuilder<Shipper> b)
    {
        b.ToTable("shippers");
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
        
        b.HasMany(e => e.Contracts).WithOne(e => e.Shipper).IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}