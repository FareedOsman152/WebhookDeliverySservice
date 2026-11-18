using Api.Common;
using Application.Order;
using MediatR;

namespace Api.Endpoints;

public static class OrderFromMarketplaceEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/ship-order", async (ShipOrderCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return ToHttpResponse.Return(result);
        });
    }
}
