using Domain;
using Domain.Order;

namespace Application.Order;

public interface IMarketplaceOrderRepository
{
    public Task Add(MarketplaceOrder order, CancellationToken ct);
    public Task<MarketplaceOrder?> GetById(Guid orderId, CancellationToken ct);
    public Task<bool> IsExist(Guid orderId, CancellationToken ct);
    Task<bool> IsExist(string marketplaceOrderId, Guid shippingContractId, CancellationToken ct);
}