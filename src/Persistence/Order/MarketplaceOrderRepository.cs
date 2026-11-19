using Application;
using Application.Order;
using Domain;
using Domain.Order;
using Domain.Order.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Order;

public class MarketplaceOrderRepository(ApplicationDbContext db, ILogger<MarketplaceOrderRepository> logger): IMarketplaceOrderRepository
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

    public async Task<MarketplaceOrder?> GetOrderWithEvents(Guid orderId, CancellationToken ct)
    {
        return await db.Orders.Include(o => o.Events).FirstOrDefaultAsync(o => o.Id == orderId, ct);
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
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException e)
        {
            foreach (var entry in e.Entries)
            {
                var attemptedChanges = string.Join(", ", entry.Properties
                    .Where(p => p.IsModified)
                    .Select(p => $"{p.Metadata.Name}: {Format(p.OriginalValue)} -> {Format(p.CurrentValue)}"));

                var databaseValues = await entry.GetDatabaseValuesAsync(ct);
                var databaseChanges = databaseValues == null
                    ? "row was deleted"
                    : string.Join(", ", databaseValues.Properties
                        .Where(p => !Equals(Format(entry.OriginalValues[p]), Format(databaseValues[p])))
                        .Select(p => $"{p.Name}: {Format(entry.OriginalValues[p])} -> {Format(databaseValues[p])}"));

                logger.LogWarning(
                    "Concurrency conflict on {EntityType} {OrderId}. Attempted changes: [{AttemptedChanges}]. Changed in database since load: [{DatabaseChanges}]",
                    entry.Metadata.ClrType.Name, order.Id, attemptedChanges, databaseChanges);
            }

            throw new OrderConcurrencyException($"Order {order.Id} was updated by another process", e);
        }
    }

    private static string? Format(object? value)
    {
        return value is byte[] bytes ? Convert.ToHexString(bytes) : value?.ToString();
    }
}