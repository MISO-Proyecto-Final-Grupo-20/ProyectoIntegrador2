using MiniValidation;
using StoreFlow.Ventas.API.Servicios;

namespace StoreFlow.Ventas.API.EndPoints;

public static class PedidosEndPoints
{
    public static void MapCrearPedidoEndPont(this IEndpointRouteBuilder app)
    {
        app.MapPost("/pedido", async (CrearPedidoCommand crearPedido, IPedidoService pedidoService) =>
        {
            if (!MiniValidator.TryValidate(crearPedido, out var errors))
                return Results.ValidationProblem(errors);

            Resultado<CrearPedidoResponse> resultado = await pedidoService.CrearPedidoAsync(crearPedido);

            if (!resultado.EsExitoso)
                return Results.BadRequest(resultado.Error);

            return Results.Accepted();
        });
    }
}

public record CrearPedidoResponse();

public record CrearPedidoCommand();