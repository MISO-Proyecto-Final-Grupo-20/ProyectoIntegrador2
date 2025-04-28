using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Inventarios;
using StoreFlow.Inventarios.API.Datos;

namespace StoreFlow.Inventarios.API.Consumidores;

public class ConsumidorValidarInventario(ILogger<ConsumidorValidarInventario> logger, InventariosDbContext dbContext)
    : IConsumer<ValidarInventario>
{
    public async Task Consume(ConsumeContext<ValidarInventario> context)
    {
        var solicitudValiada = await dbContext.ValidarPedidoConInventarioAsync(context.Message.Solicitud);
        
        await context.Publish(new InventarioValidado(context.Message.IdProceso, solicitudValiada));
    }
}