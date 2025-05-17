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
            await servicio.RegistrarCompraAsync(dto);
            return Results.Ok(new
            {
                mensaje = "Compra registrada exitosamente.",
                totalProductos = dto.Productos.Sum(p => p.Cantidad)
            });
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