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
        catch (OrderConcurrencyException e)
        {
            // TODO: a retried SaveChanges that already committed lands here too (points-to-be-handled #1): our own first attempt changed Status, so it is classified as OrderNotReadyToShip, logged as a lost race, and the order stays AttemptingToShip and is never shipped.
            switch (e.Result)
            {
                case OrderNotReadyToShip:
                    logger.LogWarning("Order {OrderId} has race condition, it is no longer ready to ship", orderId);
                    return;
                case OrderUpdateConflict:
                    logger.LogWarning("Order {OrderId} has race condition, the job will be retried", orderId);
                    throw;
            }
        }
        // TODO: Ship the order
    }
}