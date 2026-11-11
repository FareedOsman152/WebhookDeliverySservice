using Application.Marketplaces;
using Domain.Marketplaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Marketplaces;

public class MarketplaceRepository(ApplicationDbContext db): IMarketplaceRepository
{
    public async Task Add(Marketplace marketplace, CancellationToken ct)
    {
        db.Marketplaces.Add(marketplace);
        await db.SaveChangesAsync(ct);
    }

    public async Task<Marketplace?> GetByTradeLicense(string tradeLicense, CancellationToken ct)
    {
        return await db.Marketplaces.FirstOrDefaultAsync(m => m.TradeLicenseNumber == tradeLicense, ct);
    }
}