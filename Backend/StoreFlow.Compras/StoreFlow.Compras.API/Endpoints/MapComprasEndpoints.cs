using StoreFlow.Compras.API.DTOs;

namespace StoreFlow.Compras.API.Endpoints;

public static class ComprasEndpoints
{
    public static void MapComprasEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/fabricantes", async (CrearFabricanteRequest crearFabricanteDto) =>
        {
            return Results.Ok();
        });
    }
}