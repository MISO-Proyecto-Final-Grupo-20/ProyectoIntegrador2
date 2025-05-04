using Microsoft.EntityFrameworkCore;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.DTOs;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.API.EndPoints;

public static class PlanesVentaEndPoints
{
    public static void MapPlanesVentaEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/periodosTiempo", async (VentasDbContext dbContext) =>
        {
            var periodos = await dbContext.PeriodosTiempo
                .Select(p => new PeriodoTiempoDto(p.Id, p.Nombre))
                .ToListAsync();
                
            return Results.Ok(periodos);
        });
        
        var planesVentaGroup = app.MapGroup("/planesVenta");
        
        planesVentaGroup.MapGet("/", async (VentasDbContext dbContext) =>
        {
            var planes = await dbContext.PlanesVenta
                .Include(p => p.PeriodoTiempo)
                .Select(p => new PlanVentaDto(
                    p.Id, 
                    p.Nombre, 
                    p.Descripcion, 
                    p.Precio, 
                    p.PeriodoTiempoId, 
                    p.PeriodoTiempo.Nombre))
                .ToListAsync();
                
            return Results.Ok(planes);
        }).RequireAuthorization("SoloUsuariosCcp");
        
        planesVentaGroup.MapGet("/{id}", async (int id, VentasDbContext dbContext) =>
        {
            var plan = await dbContext.PlanesVenta
                .Include(p => p.PeriodoTiempo)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (plan == null)
                return Results.NotFound();
                
            var planDto = new PlanVentaDto(
                plan.Id,
                plan.Nombre,
                plan.Descripcion,
                plan.Precio,
                plan.PeriodoTiempoId,
                plan.PeriodoTiempo.Nombre);
                
            return Results.Ok(planDto);
        }).RequireAuthorization("SoloUsuariosCcp");
        
        planesVentaGroup.MapPost("/", async (CrearPlanVentaDto planDto, VentasDbContext dbContext) =>
        {
            // Verificar que exista el periodo de tiempo
            var periodoTiempo = await dbContext.PeriodosTiempo.FindAsync(planDto.PeriodoTiempoId);
            if (periodoTiempo == null)
                return Results.BadRequest($"El periodo de tiempo con ID {planDto.PeriodoTiempoId} no existe");
                
            var nuevoPlan = new PlanVenta(
                planDto.Nombre,
                planDto.Descripcion,
                planDto.Precio,
                planDto.PeriodoTiempoId);
                
            dbContext.PlanesVenta.Add(nuevoPlan);
            await dbContext.SaveChangesAsync();
            
            await dbContext.Entry(nuevoPlan).Reference(p => p.PeriodoTiempo).LoadAsync();
            
            var planCreado = new PlanVentaDto(
                nuevoPlan.Id,
                nuevoPlan.Nombre,
                nuevoPlan.Descripcion,
                nuevoPlan.Precio,
                nuevoPlan.PeriodoTiempoId,
                nuevoPlan.PeriodoTiempo.Nombre);
                
            return Results.Created($"/planesVenta/{nuevoPlan.Id}", planCreado);
        }).RequireAuthorization("SoloUsuariosCcp");
        
        planesVentaGroup.MapPut("/{id}", async (int id, ActualizarPlanVentaDto planDto, VentasDbContext dbContext) =>
        {
            var plan = await dbContext.PlanesVenta.FindAsync(id);
            if (plan == null)
                return Results.NotFound();
                
            var periodoTiempo = await dbContext.PeriodosTiempo.FindAsync(planDto.PeriodoTiempoId);
            if (periodoTiempo == null)
                return Results.BadRequest($"El periodo de tiempo con ID {planDto.PeriodoTiempoId} no existe");
                
            typeof(PlanVenta).GetProperty(nameof(PlanVenta.Nombre))?.SetValue(plan, planDto.Nombre);
            typeof(PlanVenta).GetProperty(nameof(PlanVenta.Descripcion))?.SetValue(plan, planDto.Descripcion);
            typeof(PlanVenta).GetProperty(nameof(PlanVenta.Precio))?.SetValue(plan, planDto.Precio);
            typeof(PlanVenta).GetProperty(nameof(PlanVenta.PeriodoTiempoId))?.SetValue(plan, planDto.PeriodoTiempoId);
                
            await dbContext.SaveChangesAsync();
            
            await dbContext.Entry(plan).Reference(p => p.PeriodoTiempo).LoadAsync();
            
            var planRespuesta = new PlanVentaDto(
                plan.Id,
                plan.Nombre,
                plan.Descripcion,
                plan.Precio,
                plan.PeriodoTiempoId,
                plan.PeriodoTiempo.Nombre);
                
            return Results.Ok(planRespuesta);
        }).RequireAuthorization("SoloUsuariosCcp");
        
        planesVentaGroup.MapDelete("/{id}", async (int id, VentasDbContext dbContext) =>
        {
            var plan = await dbContext.PlanesVenta.FindAsync(id);
            if (plan == null)
                return Results.NotFound();
                
            dbContext.PlanesVenta.Remove(plan);
            await dbContext.SaveChangesAsync();
            
            return Results.NoContent();
        }).RequireAuthorization("SoloUsuariosCcp");
    }
}