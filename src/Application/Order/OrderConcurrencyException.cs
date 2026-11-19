namespace Application.Order;

public class OrderConcurrencyException(string message, OrderUpdateResult result, Exception innerException) : Exception(message, innerException)
{
    public OrderUpdateResult Result { get; } = result;
}
