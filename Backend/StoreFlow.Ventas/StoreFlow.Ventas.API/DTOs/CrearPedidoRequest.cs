using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Ventas.API.EndPoints;

namespace StoreFlow.Ventas.API.DTOs;

public record CrearPedidoRequest(ProductoPedidoRequest[] ProductosPedidos)
{
    public SolicitudDePedido CrearSolicitud(int idCliente, DateTime fechaCreacion, int? idVendedor)
    {
        return new SolicitudDePedido(idCliente, fechaCreacion, ProductosPedidos.Select(x => x.CrearProductoPedido()).ToArray(), idVendedor);
    }
}