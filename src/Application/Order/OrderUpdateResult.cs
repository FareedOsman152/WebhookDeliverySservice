namespace Application.Order;

public record OrderNotReadyToShip;

public record OrderUpdateConflict;

public union OrderUpdateResult(OrderNotReadyToShip, OrderUpdateConflict);
