using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.DTOs;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.API.EndPoints;

public static class PlanesDeVentaEndPoints
{
    public static void MapPlanesDeVentasEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/planesDeVentas",
            async (CrearPlanVentaRequest request, VentasDbContext dbContext) =>
            {
                var planesDeVentas = PlanDeVentas.CrearPlanesDeVentas(request);
                await dbContext.GuardarPlanesDeVentas(planesDeVentas);
                
                return Results.Ok();
            }).RequireAuthorization("SoloUsuariosCcp");

        app.MapGet("/periodosTiempo", () =>
        {
            var periodos = Enum.GetValues(typeof(Periodicidad))
                .Cast<Periodicidad>()
                .Select(p => new PeriodoTiempoResponse((int)p, p.ToString()))
                .ToList();
            return Results.Ok(periodos);
            
        });
    }
}