using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Logistica;

public record ProgramarEntrega(Guid IdProceso, PedidoResponse Pedido);

public record EntregaProgramada(Guid IdProceso);