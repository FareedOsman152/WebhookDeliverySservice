using Domain.Order.Enums;

namespace Domain;

public sealed class OrderEvent: BaseEntity
{
    private OrderEvent() {}// EF Core
    
    internal OrderEvent(
        Guid orderId,
        OrderEventType eventType,
        OrderStatus? statusBefore,
        OrderStatus statusAfter)
    {
        ArgumentNullException.ThrowIfNull(eventType);
        ArgumentNullException.ThrowIfNull(statusAfter);

        if (orderId == Guid.Empty)
            throw new ArgumentException("OrderId is required.", nameof(orderId));

        OrderId = orderId;
        EventType = eventType;
        StatusBefore = statusBefore;
        StatusAfter = statusAfter;
    }

    public OrderEventType EventType { get; private set; }
    public OrderStatus? StatusBefore { get; private set; }
    public OrderStatus StatusAfter { get; private set; }

    public Guid OrderId { get; private set; }
    public MarketplaceOrder Order { get; private set; }
}
