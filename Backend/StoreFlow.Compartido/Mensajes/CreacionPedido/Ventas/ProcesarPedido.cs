using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;

namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

public record ProcesarPedido(
    Guid IdProceso,
    SolicitudDePedido solicitud
);

public record SolicitudDePedido(int IdCliente, DateTime FechaCreacion, ProductoSolicitado[] productosSolicitados);

public record ProductoSolicitado(int Id, int Cantidad, decimal Precio, bool TieneInventario);


public record RegistrarPedido(
    Guid IdProceso,
    SolicitudDePedido SolicitudValiada,
    List<InformacionPoducto> InformacionProductos,
    InformacionCliente InformacionCliente,
    InformacionVendedor? InformacionVendedor);

public record PedidoRegistrado(Guid IdProceso);



