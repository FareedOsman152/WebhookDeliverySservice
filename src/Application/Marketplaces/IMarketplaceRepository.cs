using Domain;
using Domain.Marketplaces;

namespace Application.Marketplaces;

public interface IMarketplaceRepository
{
    public Task Add(Marketplace marketplace, CancellationToken ct);
    public Task<Marketplace?> GetByTradeLicense(string tradeLicense, CancellationToken ct);
}