using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using Microsoft.AspNetCore.Http;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Ventas.API.EndPoints;

public static class PedidosEndPoints
{
    public static void MapCrearPedidoEndPont(this IEndpointRouteBuilder app)
    {
        app.MapPost("/pedido", async (HttpContext httpContext, CrearPedidoRequest crearPedido, IPublishEndpoint publishEndpoint, IDateTimeProvider dateTimeProvider) =>
        {
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var idUsuario = int.Parse(jwtToken.Claims.First(c => c.Type == "idUsuario").Value);
            
            var solicitud = crearPedido.CrearSolicitud(idUsuario, dateTimeProvider.UtcNow);
            var procesarPedido = new ProcesarPedido(Guid.CreateVersion7(), solicitud);

            await publishEndpoint.Publish(procesarPedido);
            
            return Results.Accepted();
        }).RequireAuthorization("Cliente");
    }
}

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}

public record CrearPedidoResponse();


public record CrearPedidoRequest(ProductoPedidoRequest[] productosPedidos)
{
    public SolicitudDePedido CrearSolicitud(int idUsuario, DateTime fechaCreacion)
    {
        return new SolicitudDePedido(idUsuario, fechaCreacion, productosPedidos.Select(x => x.CrearProductoPedido()).ToArray());
    }
}

public record ProductoPedidoRequest(string Codigo, int Cantidad, decimal Precio)
{
    public ProductoSolicitado CrearProductoPedido()
    {
        if(int.TryParse(Codigo, out int codigo) == false)
        {
            throw new ArgumentException("El código del producto no es válido.");
        }
        return new ProductoSolicitado(codigo, Cantidad, Precio, false);
    }
};





