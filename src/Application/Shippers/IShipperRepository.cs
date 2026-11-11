using Domain.Shippers;

namespace Application.Shippers;

public interface IShipperRepository
{
    public Task Add(Shipper shipper, CancellationToken ct);
    public Task<Shipper?> GetByTradeLicense(string tradeLicense, CancellationToken ct);
}