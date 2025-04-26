using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Ventas.API.Consumidores;

public class ConsumidorConfirmarPedido : IConsumer<ConfirmarPedido>
{
    private readonly ILogger<ConsumidorConfirmarPedido> _logger;

    public ConsumidorConfirmarPedido(ILogger<ConsumidorConfirmarPedido> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ConfirmarPedido> context)
    {
        _logger.LogInformation($"Pedido confirmado: {context.Message.IdPedido}");
    }
}