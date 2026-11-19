namespace Application.Order;

public class OrderConcurrencyException(string message, Exception innerException) : Exception(message, innerException);
