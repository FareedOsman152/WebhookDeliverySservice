using Domain;

namespace Application.Order;

public interface IMarketplaceOrderRepository
{
    public Task Add(MarketplaceOrder order, CancellationToken ct);
    public Task<MarketplaceOrder?> GetById(Guid orderId, CancellationToken ct);
}