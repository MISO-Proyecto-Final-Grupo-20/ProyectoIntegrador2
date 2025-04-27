using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Ventas.API.EndPoints;

public static class PedidosEndPoints
{
    public static void MapCrearPedidoEndPont(this IEndpointRouteBuilder app)
    {
        app.MapPost("/pedido", async (CrearPedidoRequest crearPedido, IPublishEndpoint publishEndpoint) =>
        {
            var solicitud = crearPedido.CrearSolicitud();
            var procesarPedido = new ProcesarPedido(Guid.CreateVersion7(), solicitud);

            await publishEndpoint.Publish(procesarPedido);
            
            return Results.Accepted();
        });
    }
}

public record CrearPedidoResponse();


public record CrearPedidoRequest(int IdCliente, ProductoPedidoRequest[] productosPedidos)
{
    public SolicitudDePedido CrearSolicitud()
    {
        return new SolicitudDePedido(IdCliente, productosPedidos.Select(x => x.CrearProductoPedido()).ToArray());
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
        return new ProductoSolicitado(codigo, Cantidad, Precio);
    }
};





