using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Logistica;

namespace StoreFlow.Logistica.API.Consumidores;

public class ConsumidorPreperarEnvioPedido : IConsumer<PrepararEnvioPedido>
{
    private readonly ILogger<ConsumidorPreperarEnvioPedido> _logger;

    public ConsumidorPreperarEnvioPedido(ILogger<ConsumidorPreperarEnvioPedido> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PrepararEnvioPedido> context)
    {
        _logger.LogInformation("Preparando envío del pedido {idproceso}", context.Message.idProceso);
        await context.Publish(new PedidoPreparadoParaEnvio(context.Message.idProceso));
    }
}