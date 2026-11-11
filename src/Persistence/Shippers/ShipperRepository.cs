using Application.Shippers;
using Domain.Shippers;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Shippers;

public class ShipperRepository(ApplicationDbContext db): IShipperRepository
{
    public async Task Add(Shipper shipper, CancellationToken ct)
    {
        db.Shippers.Add(shipper);
        await db.SaveChangesAsync(ct);
    }

    public async Task<Shipper?> GetByTradeLicense(string tradeLicense, CancellationToken ct)
    {
        return await db.Shippers.FirstOrDefaultAsync(s => s.TradeLicenseNumber == tradeLicense, ct);
    }
}