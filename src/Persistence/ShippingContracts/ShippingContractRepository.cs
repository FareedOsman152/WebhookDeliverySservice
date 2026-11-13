using Application.ShippingContracts;
using Domain.ShippingContracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ShippingContracts;

public sealed class ShippingContractRepository(ApplicationDbContext db) : IShippingContractRepository
{
    public async Task Add(ShippingContract contract, CancellationToken ct)
    {
        db.ShippingContracts.Add(contract);
        await db.SaveChangesAsync(ct);
    }

    public async Task<ShippingContract?> GetActiveContract(Guid contractId, CancellationToken ct)
    {
        return await db.ShippingContracts.FirstOrDefaultAsync(c => c.Id == contractId && c.Status == ShippingContractStatus.Active, ct);
    }

    public async Task<ShippingContract?> GetActiveContract(string marketplaceTradeLicense, string shipperTradeLicense, CancellationToken ct)
    {
        return await db.ShippingContracts
            .Where(c => c.Marketplace.TradeLicenseNumber == marketplaceTradeLicense
                        && c.Shipper.TradeLicenseNumber == shipperTradeLicense
                        && c.Status == ShippingContractStatus.Active)
            .SingleOrDefaultAsync(ct);
    }
    
    public async Task<bool> IsActiveContractExist(string marketplaceTradeLicense, string shipperTradeLicense, CancellationToken ct)
    {
        return await db.ShippingContracts
            .AnyAsync(c => c.Marketplace.TradeLicenseNumber == marketplaceTradeLicense
                           && c.Shipper.TradeLicenseNumber == shipperTradeLicense
                           && c.Status == ShippingContractStatus.Active
                           , ct);
    }
}