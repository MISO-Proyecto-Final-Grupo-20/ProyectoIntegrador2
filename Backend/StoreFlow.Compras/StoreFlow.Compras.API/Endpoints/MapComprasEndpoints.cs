using MiniValidation;
using StoreFlow.Compras.API.DTOs;

namespace StoreFlow.Compras.API.Endpoints;

public static class ComprasEndpoints
{
    public static void MapComprasEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/fabricantes", async (CrearFabricanteRequest crearFabricanteDto) =>
        {
            if (!MiniValidator.TryValidate(crearFabricanteDto, out var errors))
                return Results.ValidationProblem(errors);

            return Results.Ok();
        });
    }
}