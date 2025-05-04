using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Ventas.API.EndPoints;

namespace StoreFlow.Ventas.API.DTOs;

public record CrearPedidoRequest(ProductoPedidoRequest[] ProductosPedidos)
{
    public SolicitudDePedido CrearSolicitud(int idUsuario, DateTime fechaCreacion)
    {
        return new SolicitudDePedido(idUsuario, fechaCreacion, ProductosPedidos.Select(x => x.CrearProductoPedido()).ToArray());
    }
}