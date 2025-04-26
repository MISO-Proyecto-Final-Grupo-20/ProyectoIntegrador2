using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Ventas.API.EndPoints;

public static class PedidosEndPoints
{
    public static void MapCrearPedidoEndPont(this IEndpointRouteBuilder app)
    {
        app.MapPost("/pedido", async (CrearPedidoCommand crearPedido, IPublishEndpoint publishEndpoint) =>
        {
            var mensajeProcesarPeido = new ProcesarPedido(Guid.CreateVersion7(), 1);
            await publishEndpoint.Publish(mensajeProcesarPeido);
            return Results.Accepted();
        });
    }
}

public record CrearPedidoResponse();

public record CrearPedidoCommand();