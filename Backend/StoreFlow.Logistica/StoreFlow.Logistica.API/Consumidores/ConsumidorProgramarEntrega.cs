using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Logistica;

namespace StoreFlow.Logistica.API.Consumidores;

public class ConsumidorProgramarEntrega : IConsumer<ProgramarEntrega>
{
    public Task Consume(ConsumeContext<ProgramarEntrega> context)
    {
        throw new NotImplementedException();
    }
}