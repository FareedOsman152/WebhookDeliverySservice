using Application;
using Application.Order;
using Domain;
using Domain.Order;
using Domain.Order.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
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
        // we may will add order by LastModifiedOn and using take to just take a chunk
        // just get where attempt less than 10
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
            OrderUpdateResult result = new OrderUpdateConflict();

            foreach (var entry in e.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(ct);
                var changedInDatabase = GetChangedInDatabase(entry, databaseValues);

                logger.LogWarning(
                    "Concurrency conflict on {EntityType} {OrderId}. Attempted changes: [{AttemptedChanges}]. Changed in database since load: [{DatabaseChanges}]",
                    entry.Metadata.ClrType.Name, order.Id, DescribeAttemptedChanges(entry), DescribeDatabaseChanges(entry, databaseValues, changedInDatabase));

                if (IsOrderStatusChanged(entry, changedInDatabase))
                    result = new OrderNotReadyToShip();
            }

            throw new OrderConcurrencyException($"Order {order.Id} was updated by another process", result, e);
        }
    }

    private static List<IProperty> GetChangedInDatabase(EntityEntry entry, PropertyValues? databaseValues)
    {
        if (databaseValues == null)
            return [];

        return databaseValues.Properties
            .Where(p => !Equals(Format(entry.OriginalValues[p]), Format(databaseValues[p])))
            .ToList();
    }

    private static bool IsOrderStatusChanged(EntityEntry entry, List<IProperty> changedInDatabase)
    {
        return entry.Entity is MarketplaceOrder
            && changedInDatabase.Any(p => p.Name == nameof(MarketplaceOrder.Status));
    }

    private static string DescribeAttemptedChanges(EntityEntry entry)
    {
        return string.Join(", ", entry.Properties
            .Where(p => p.IsModified)
            .Select(p => $"{p.Metadata.Name}: {Format(p.OriginalValue)} -> {Format(p.CurrentValue)}"));
    }

    private static string DescribeDatabaseChanges(EntityEntry entry, PropertyValues? databaseValues, List<IProperty> changedInDatabase)
    {
        if (databaseValues == null)
            return "row was deleted";

        return string.Join(", ", changedInDatabase
            .Select(p => $"{p.Name}: {Format(entry.OriginalValues[p])} -> {Format(databaseValues[p])}"));
    }

    private static string? Format(object? value)
    {
        return value is byte[] bytes ? Convert.ToHexString(bytes) : value?.ToString();
    }
}