using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;
using StoreFlow.Compras.API.Servicios;

namespace StoreFlow.Compras.API.Consumidores;

public class ConsumidorObtenerInformacionProductos(ILogger<ConsumidorObtenerInformacionProductos> logger, IProductosService productosService) : IConsumer<ObtenerInformacionProductos>
{
    public async Task Consume(ConsumeContext<ObtenerInformacionProductos> context)
    {
        logger.LogInformation("Iniciando la obtención de información de productos para el proceso {IdProceso}", context.Message.IdProceso);
        var informacionProductos = await productosService.ObtenerProductosAsync(context.Message.IdsProductos);

        var informacionProductoObtenida = new InformacionProductoObtenida(context.Message.IdProceso, informacionProductos);
        
        logger.LogInformation("Se obtuvo la información de productos para el proceso {@info}", informacionProductoObtenida);
        
        await context.Publish(informacionProductoObtenida);
    }
}