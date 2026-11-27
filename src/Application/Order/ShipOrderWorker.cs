using Application.BackgroundJobs;
using Domain.Order;
using Microsoft.Extensions.Logging;

namespace Application.Order;

// this service will be inhert from BackGroundService => then it will have while loop => the most simple way for now
public class ShipOrderWorker(IMarketplaceOrderRepository ordersRepo, IBackgroundJobService backgroundJobService, ILogger<ShipOrderWorker> logger)
{
    // dictionary <shipper trade license/id, ShipperService> 
    // when one shipper is down => we can't just use task.sleep => we are blocking other shippers!
    // we will create a worker for each shipper => that means duplicated code
    // we will have IShipperService for example and one ShipperServices => with static class represente {shipperid, shipper url endpoints, readyOn} => so here ouer worker will assign readyOn to DateTime.Now + 10s, in the second rownd, the query will cehck the DateTime.Now > readyOn => faild again, update the time again => ti's rare, and down will not be for a long time usually! 
    public async Task CheckReadyToShippingOrders()
    {
        var orders = await ordersRepo.GetReadyToShippingOrders(CancellationToken.None);
        if (orders.Count == 0)
            return;

        // the queue is FIFO => so the earlier modifid on we will take
        // we will select 10 orders for each shipper (output => attemptingToShipping++(change LastModifiedOn), insert new event attempt to shipping (if this is just in db level we need to add date and others that we already do in savechanges override)) => and based on this no need for status AttemptingToShip, becasue we already have another column to know this info
        // sned to each shipper the 10 orders
        // all failed, don't care just fo to the second loop, and the faild will be moved to the end of the queue
        // some succeded => update no problem
        // sime faild => update no problem

        // if our system crashed after getting the chunk and updating it => mostly no problem becasue we just update the attempting => but here we need to determine if the attempted was becasue of the shipper or pur system crashed? becasue if our system crashed we can't just say it's attempt => how to do? just make it as todo
        
        // if the attempt goes to 10 and also faild for any reason => nothing to do, another worker will handle this.

        // if any shipper system is down (not just a 400 (refused)) => update its contract IsDown false => some worker will update it after a time, or the orker will test the shipper endpoiints to check if it came up or not
        // if the shipper refused => update the order to refused then nothing to do, another worker will handler refused orders
        // I think there is no any other case, cases like 401 we don't care about for now, and if we have it (we don't hav auth) => we will just delegate to another api to get token or any thing else...


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