namespace Application.Order;

public class DuplicateOrderException(string message, Exception innerException) : Exception(message, innerException);
