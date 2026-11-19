using Application.BackgroundJobs;
using Domain.Order;
using Microsoft.Extensions.Logging;

namespace Application.Order;

public class ShipOrderWorker(IMarketplaceOrderRepository ordersRepo, IBackgroundJobService backgroundJobService, ILogger<ShipOrderWorker> logger)
{
    public async Task CheckReadyToShippingOrders()
    {
        var orders = await ordersRepo.GetReadyToShippingOrders(CancellationToken.None);
        if (orders.Count == 0)
            return;

        // TODO: orders that already have a pending job are enqueued again on every check (points-to-be-handled #2).
        foreach (var order in orders)
        {
            backgroundJobService.Enqueue<ShipOrderWorker>(w => w.ShipOrder(order.Id));
        }
    }

    public async Task ShipOrder(Guid orderId)
    {
        var ct = CancellationToken.None;
        var order = await ordersRepo.GetOrderWithEvents(orderId, ct);

        try
        {
            order!.AttemptToShipOrder();
        }
        catch (InvalidOperationException e)
        {
            logger.LogWarning("Order {OrderId} can't be shipped: {Reason}", orderId, e.Message);
            return;
        }

        try
        {
            await ordersRepo.Update(order, ct);
        }
        catch (OrderConcurrencyException)
        {
            // TODO: a retried SaveChanges that already committed lands here too (points-to-be-handled #1); the order stays AttemptingToShip and is never shipped.
            logger.LogWarning("Order {OrderId} has race condition", orderId);
            return;
        }
        // TODO: Ship the order
    }
}