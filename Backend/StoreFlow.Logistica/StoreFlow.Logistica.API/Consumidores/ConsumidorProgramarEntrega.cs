using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Logistica;
using StoreFlow.Logistica.API.Servicios;

namespace StoreFlow.Logistica.API.Consumidores;

public class ConsumidorProgramarEntrega(IEntregaServicio entregaServicio) : IConsumer<ProgramarEntrega>
{
    public async Task Consume(ConsumeContext<ProgramarEntrega> context)
    {
        await entregaServicio.GuardarEntregaAsync(context.Message.Pedido);
        await context.Publish(new EntregaProgramada(context.Message.IdProceso));
    }
}