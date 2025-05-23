﻿using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using StoreFlow.Compartidos.Core.Infraestructura;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.DTOs;

namespace StoreFlow.Ventas.API.EndPoints;

public static class PedidosEndPoints
{
    public static void MapCrearPedidoEndPont(this IEndpointRouteBuilder app)
    {
        app.MapPost("/pedidos",
            async (HttpContext httpContext, ProductoPedidoRequest[] crearPedido, IPublishEndpoint publishEndpoint,
                IDateTimeProvider dateTimeProvider) =>
            {
                var idUsuario = UtilidadesEndPoints.RecuperarIdUsuarioToken(httpContext);

                var pedidoRequest = new CrearPedidoRequest(crearPedido);

                var solicitud = pedidoRequest.CrearSolicitud(idUsuario, dateTimeProvider.UtcNow, null);
                var procesarPedido = new ProcesarPedido(Guid.CreateVersion7(), solicitud);

                await publishEndpoint.Publish(procesarPedido);

                return Results.Accepted(null, idUsuario);
            }).RequireAuthorization("Cliente");

        app.MapPost("/pedidos/{idCliente:int}", async (HttpContext httpContext, int idCliente,
            ProductoPedidoRequest[] crearPedido, IPublishEndpoint publishEndpoint,
            IDateTimeProvider dateTimeProvider) =>
        {
            var idUsuario = UtilidadesEndPoints.RecuperarIdUsuarioToken(httpContext);

            var pedidoRequest = new CrearPedidoRequest(crearPedido);
            var solicitud = pedidoRequest.CrearSolicitud(idCliente, dateTimeProvider.UtcNow, idUsuario);
            var procesarPedido = new ProcesarPedido(Guid.CreateVersion7(), solicitud);

            await publishEndpoint.Publish(procesarPedido);

            return Results.Accepted(null, idUsuario);
        }).RequireAuthorization("Vendedor");

        app.MapGet("/pedidos/pendientes", async (HttpContext httpContext, VentasDbContext ventasDbContext) =>
        {
            var idUsuario = UtilidadesEndPoints.RecuperarIdUsuarioToken(httpContext);

            var pedidos = await ventasDbContext.ObtenerPedidosAsync(idUsuario);

            return Results.Ok(pedidos);
        }).RequireAuthorization("Cliente");

        app.MapGet("/pedidos/pendientes/{idCliente:int}",
            async (HttpContext httpContext, int idCliente, VentasDbContext ventasDbContext) =>
            {
                var idVendedor = UtilidadesEndPoints.RecuperarIdUsuarioToken(httpContext);

                var pedidos = await ventasDbContext.ObtenerPedidosAsync(idCliente, idVendedor);

                return Results.Ok(pedidos);
            }).RequireAuthorization("Vendedor");

        app.MapPost("/consultaInformes", async (ReporteVentasRequest request, VentasDbContext ventasDbContext) =>
        {
            var report = await ventasDbContext.ObtenerReporteVentasAsync(request);
            return Results.Ok(report);
        }).RequireAuthorization("SoloUsuariosCcp");
    }
}