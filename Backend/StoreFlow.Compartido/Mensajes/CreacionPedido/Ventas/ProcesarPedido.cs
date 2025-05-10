using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;

namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

public record ProcesarPedido(
    Guid IdProceso,
    SolicitudDePedido Solicitud
);

public record SolicitudDePedido(int IdCliente, DateTime FechaCreacion, ProductoSolicitado[] ProductosSolicitados, int? IdVendedor);

public record ProductoSolicitado(int Id, int Cantidad, decimal Precio, bool TieneInventario);


public record RegistrarPedido(
    Guid IdProceso,
    SolicitudDePedido SolicitudValiada,
    List<InformacionPoducto> InformacionProductos,
    InformacionCliente InformacionCliente,
    InformacionVendedor? InformacionVendedor);

public record PedidoRegistrado(Guid IdProceso, PedidoResponse pedido);

public record PedidoResponse(int Numero, int IdCliente, DateTime FechaRegistro, DateTime FechaEntrega, string LugarEntrega, EstadoPedido Estado,   ProductoPedidoResponse[] Productos, decimal Total, string NombreCliente);


public record ProductoPedidoResponse(int Id, int Cantidad, decimal Precio, string? Codigo, string? Nombre, string? Imagen);

public enum EstadoPedido
{
    Pendiente = 0
}




