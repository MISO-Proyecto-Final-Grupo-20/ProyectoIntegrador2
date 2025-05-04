using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.API.Consumidores;

public class ConsumidorConfirmarPedido(ILogger<RegistrarPedido> logger, VentasDbContext ventasDbContext)
    : IConsumer<RegistrarPedido>
{

    public async Task Consume(ConsumeContext<RegistrarPedido> context)
    {
        var solicitud = context.Message.SolicitudValiada;
        var pedido = new Pedido(solicitud, context.Message.InformacionProductos);

        await ventasDbContext.GuardarPedidoAsync(pedido);
    }
}