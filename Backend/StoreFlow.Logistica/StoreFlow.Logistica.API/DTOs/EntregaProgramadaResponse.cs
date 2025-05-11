using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Logistica.API.DTOs;

public record EntregaProgramadaResponse(int Id, int Numero, DateTime FechaEntrega, string LugarEntrega, ProductoPedidoResponse[] Productos);
