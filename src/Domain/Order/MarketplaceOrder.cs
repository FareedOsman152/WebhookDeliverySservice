using Ardalis.SmartEnum;
using Domain.Order.Enums;
using Domain.ShippingContracts;
using Shared;

namespace Domain;

public sealed class MarketplaceOrder: AuditableEntity
{
    private MarketplaceOrder() {}// EF Core

    private MarketplaceOrder(
        string marketplaceOrderId,
        string orderDetails,
        string shippingDetails,
        Guid shippingContractId)
    {
        MarketplaceOrderId = marketplaceOrderId;
        OrderDetails = orderDetails;
        ShippingDetails = shippingDetails;
        ShippingContractId = shippingContractId;
        Status = OrderStatus.ReadyToShipping;
        _events = [];
    }
    
    public static MarketplaceOrder CreateNewOrder(string marketplaceOrderId, string orderDetails, string shippingDetails, Guid shippingContractId)
    {
        BlankCheckExtensions.ThrowIfAnyBlank(marketplaceOrderId, orderDetails, shippingDetails);
        var newOrder =  new MarketplaceOrder(marketplaceOrderId, orderDetails, shippingDetails, shippingContractId);
        newOrder._events.Add(new OrderEvent(newOrder.Id, OrderEventType.MarketplaceSentNewOrder, null, OrderStatus.ReadyToShipping));
        
        return newOrder;
    }
    
    // methods for update the order with the several events, will be done after ef configuration.
    
    public string MarketplaceOrderId { get; private set; }
    public OrderStatus Status { get; private set; }
    
    // OrderDetails and ShippingDetails are some details we don't care about in our system now, it must be documented based on a real marketplaces and shippers, so it's skipped
    public string OrderDetails { get; private set; }
    public string ShippingDetails { get; private set; }

    // in the contract we have the marketplace and shipper ids
    public Guid ShippingContractId { get; private set; } // within which shipping contract?
    public ShippingContract ShippingContract { get; private set; }
    
    public IEnumerable<OrderEvent> Events => _events;
    private List<OrderEvent> _events;
}

