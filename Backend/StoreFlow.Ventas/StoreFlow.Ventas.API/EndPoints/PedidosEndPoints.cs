using MiniValidation;
using StoreFlow.Ventas.API.Servicios;

namespace StoreFlow.Ventas.API.EndPoints;

public static class PedidosEndPoints
{
    public static void MapCrearPedidoEndPont(this IEndpointRouteBuilder app)
    {
        app.MapPost("/pedido", async (CrearPedidoCommand crearPedido) =>
        {
            return Results.Accepted();
        });
    }
}

public record CrearPedidoResponse();

public record CrearPedidoCommand();