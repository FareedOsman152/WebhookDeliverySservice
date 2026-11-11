using Application;
using Application.Order;
using Domain;

namespace Persistence.Order;

public class MarketplaceOrderRepository(ApplicationDbContext db): IMarketplaceOrderRepository
{
    public async Task Add(MarketplaceOrder order, CancellationToken ct)
    {
        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);
    }

    public async Task<MarketplaceOrder?> GetById(Guid orderId, CancellationToken ct)
    {
        return await db.Orders.FindAsync([orderId], ct);
    }
}