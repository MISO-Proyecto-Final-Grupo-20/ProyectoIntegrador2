using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Inventarios;

public record ValidarInventario(
    Guid IdProceso, SolicitudDePedido Solicitud);