using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Inventarios;

namespace StoreFlow.Inventarios.API.Consumidores;

public class ConsumidorValidarInventario : IConsumer<ValidarInventario>
{
    private readonly ILogger<ConsumidorValidarInventario> _logger;

    public ConsumidorValidarInventario(ILogger<ConsumidorValidarInventario> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ValidarInventario> context)
    {
        _logger.LogInformation($"Validando inventario para el pedido: {context.Message.IdProceso}");
        await context.Publish(new InventarioValidado(context.Message.IdProceso));
    }
}