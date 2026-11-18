using Application.ShippingContracts;
using Domain.Order;
using FluentValidation;
using MediatR;
using Shared;

namespace Application.Order;

public record ShipOrderCommand(string MarketplaceTradeLicenseNumber, string ShipperTradeLicenseNumber, string MarketplaceOrderId, string OrderDetails, string ShippingDetails) : IRequest<Result>;

public class ShipOrderHandler(IMarketplaceOrderRepository ordersRepo, IShippingContractRepository shippingContractRepo): IRequestHandler<ShipOrderCommand, Result>
{
    public async Task<Result> Handle(ShipOrderCommand command, CancellationToken ct)
    {
        var contract = await shippingContractRepo.GetActiveContract(command.MarketplaceTradeLicenseNumber, command.ShipperTradeLicenseNumber, ct);
        
        var isOrderExist = await ordersRepo.IsExist(command.MarketplaceOrderId, contract!.Id, ct);
        if (isOrderExist)
            return new Success("Order already exists");

        try
        {
            await ordersRepo.Add(MarketplaceOrder.CreateNewOrder(command.MarketplaceOrderId, command.OrderDetails, command.ShippingDetails, contract.Id), ct);
        }
        catch (DuplicateOrderException)
        {
            return new Success("Order already exists");
        }
        

        return new Success("Order received and processing");
    }
}

public class ShipOrderRequestValidator:  AbstractValidator<ShipOrderCommand>
{
    public ShipOrderRequestValidator(IShippingContractRepository shippingContractRepo)
    {
        RuleFor(m => m.MarketplaceTradeLicenseNumber).NotEmpty().NotNull();
        RuleFor(m => m.ShipperTradeLicenseNumber).NotEmpty().NotNull();
        RuleFor(m => m.OrderDetails).NotEmpty().NotNull();
        RuleFor(m => m.MarketplaceOrderId).NotEmpty().NotNull();
        RuleFor(x => new {x.MarketplaceTradeLicenseNumber, x.ShipperTradeLicenseNumber}).MustAsync(async (request, ct) =>
        {
             if (request.MarketplaceTradeLicenseNumber.IsBlank() || request.ShipperTradeLicenseNumber.IsBlank()) return false;
             
            return await shippingContractRepo.IsActiveContractExist(request.MarketplaceTradeLicenseNumber, request.ShipperTradeLicenseNumber, ct);
        });
    }
}