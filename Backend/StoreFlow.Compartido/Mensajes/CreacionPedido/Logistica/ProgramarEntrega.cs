using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Logistica;

public record ProgramarEntrega(Guid IdProceso, int IdPedido,
    SolicitudDePedido Solicitud);

public record EntregaProgramada(Guid IdProceso);