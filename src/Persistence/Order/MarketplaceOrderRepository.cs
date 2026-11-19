using Application;
using Application.Order;
using Domain;
using Domain.Order;
using Domain.Order.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Order;

public class MarketplaceOrderRepository(ApplicationDbContext db): IMarketplaceOrderRepository
{
    public async Task Add(MarketplaceOrder order, CancellationToken ct)
    {
        db.Orders.Add(order);
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException { Number: 2601 or 2627 })
        {
            throw new DuplicateOrderException("Order already exists", e);
        }
    }

    public async Task<MarketplaceOrder?> GetById(Guid orderId, CancellationToken ct)
    {
        return await db.Orders.FindAsync([orderId], ct);
    }

    public async Task<bool> IsExist(Guid orderId, CancellationToken ct)
    {
        return await db.Orders.AnyAsync(o => o.Id == orderId, ct);
    }
    
    public async Task<bool> IsExist(string marketplaceOrderId, Guid shippingContractId, CancellationToken ct)
    {
        return await db.Orders.AnyAsync(o => o.MarketplaceOrderId == marketplaceOrderId && o.ShippingContractId == shippingContractId, ct);
    }

    public async Task<List<MarketplaceOrder>> GetReadyToShippingOrders(CancellationToken ct)
    {
        return await db.Orders.Where(o => o.Status == OrderStatus.ReadyToShipping).ToListAsync(ct);
    }

    public async Task Update(MarketplaceOrder order, CancellationToken ct)
    {
        await db.SaveChangesAsync(ct);
    }
}