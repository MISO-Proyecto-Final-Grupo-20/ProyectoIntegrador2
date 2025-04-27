namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

public record ProcesarPedido(
    Guid Id,
    SolicitudDePedido solicitud
);

public record SolicitudDePedido(int IdCliente, ProductoSolicitado[] productosSolicitados);

public record ProductoSolicitado(int Id, int Cantidad, decimal Precio);
