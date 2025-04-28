namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

public record ProcesarPedido(
    Guid IdProceso,
    SolicitudDePedido solicitud
);

public record SolicitudDePedido(int IdCliente, DateTime FechaCreacion, ProductoSolicitado[] productosSolicitados);

public record ProductoSolicitado(int Id, int Cantidad, decimal Precio, bool TieneInventario);


public record RegistrarPedido(Guid IdProceso, SolicitudDePedido SolicitudValiada);

public record PedidoRegistrado(Guid IdProceso);
