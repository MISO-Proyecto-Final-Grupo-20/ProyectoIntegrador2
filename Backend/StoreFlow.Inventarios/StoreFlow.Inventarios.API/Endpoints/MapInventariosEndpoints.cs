using StoreFlow.Inventarios.API.Datos;
using StoreFlow.Inventarios.API.DTOs;

namespace StoreFlow.Inventarios.API.Endpoints;

public static class InventariosEndpoints
{
    public static void MapInventariosEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/existeProducto", async (
            ExisteProductoRequest request, 
            InventariosDbContext db) =>
            {
                var esEntero = int.TryParse(request.Codigo, out var codigo);
                if (!esEntero)
                    return Results.Ok(false);
                
                var hayInventarioSuficiente = await db.ExisteInventarioSuficienteAsync(codigo, request.Cantidad);
                return Results.Ok(hayInventarioSuficiente);
            }).RequireAuthorization();
    }
}