using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.DTOs;
using StoreFlow.Ventas.API.Servicios;

namespace StoreFlow.Ventas.API.EndPoints;

public static class PedidosEndPoints
{
    public static void MapCrearPedidoEndPont(this IEndpointRouteBuilder app)
    {
        app.MapPost("/pedidos", async (HttpContext httpContext, ProductoPedidoRequest[] crearPedido, IPublishEndpoint publishEndpoint, IDateTimeProvider dateTimeProvider) =>
        {
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var pedidoRequest = new CrearPedidoRequest(crearPedido);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var idUsuario = int.Parse(jwtToken.Claims.First(c => c.Type == "idUsuario").Value);
            
            var solicitud = pedidoRequest.CrearSolicitud(idUsuario, dateTimeProvider.UtcNow);
            var procesarPedido = new ProcesarPedido(Guid.CreateVersion7(), solicitud);
            
            await publishEndpoint.Publish(procesarPedido);
            
            return Results.Accepted(null, idUsuario);
        }).RequireAuthorization("Cliente");
        
        app.MapGet("/pedidos/pendientes", async (HttpContext httpContext, VentasDbContext ventasDbContext) =>
        {
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var idUsuario = int.Parse(jwtToken.Claims.First(c => c.Type == "idUsuario").Value);
            
            var pedidos = await ventasDbContext.ObtenerPedidosAsync(idUsuario);
            
            return Results.Ok(pedidos);
        }).RequireAuthorization("Cliente");

        app.MapPost("/", async (ReporteVentasRequest request, VentasDbContext ventasDbContext) =>
        {
            ReporteVentasResponse[] report = await ventasDbContext.ObtenerReporteVentasAsync(request);
            return Results.Ok(report);
        });


    }

    
}