using Domain.ShippingContracts;

namespace Application.ShippingContracts;

public interface IShippingContractRepository
{
    public Task Add(ShippingContract contract, CancellationToken ct);
    public Task<ShippingContract?> GetActiveContract(Guid contractId, CancellationToken ct);
    public Task<ShippingContract?> GetActiveContract(string marketplaceTradeLicense, string shipperTradeLicense, CancellationToken ct);
    Task<bool> IsActiveContractExist(string marketplaceTradeLicense, string shipperTradeLicense, CancellationToken ct);
}