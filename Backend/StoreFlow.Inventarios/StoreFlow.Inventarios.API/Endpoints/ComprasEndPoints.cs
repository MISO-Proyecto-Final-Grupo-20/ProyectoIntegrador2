using Microsoft.EntityFrameworkCore;
using StoreFlow.Inventarios.API.Datos;
using StoreFlow.Inventarios.API.DTOs;
using StoreFlow.Inventarios.API.Servicios;

namespace StoreFlow.Inventarios.API.Endpoints;

public static class ComprasEndPoints
{
    public static void MapComprasEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/compras", async (
            RegistroCompraBodegaDto dto,
            IRegistrarCompraService servicio) =>
        {
            try
            {
                if (dto.Productos is null || dto.Productos.Count == 0)
                    return Results.BadRequest("Debe registrar al menos un producto.");

                await servicio.RegistrarCompraAsync(dto);

                return Results.Ok(new
                {
                    mensaje = "Compra registrada exitosamente.",
                    totalProductos = dto.Productos.Sum(p => p.Cantidad)
                });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }).RequireAuthorization("SoloUsuariosCcp");

        app.MapGet("/bodegas", async (InventariosDbContext dbContext) =>
        {
            var bodegas = await dbContext.Bodegas
                .OrderBy(b => b.Nombre)
                .Select(b => new
                {
                    id = b.Id,
                    descripcion = b.Nombre
                })
                .ToListAsync();

            return Results.Ok(bodegas);
        }).RequireAuthorization("SoloUsuariosCcp");
    }
}