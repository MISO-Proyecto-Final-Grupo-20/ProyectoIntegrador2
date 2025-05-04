using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Usuarios.API.Datos;

namespace StoreFlow.Usuarios.API.Consumidores;

public class ObtenerInformacionClienteYVendedorConsumer(UsuariosDbContext usuariosDbContext) : IConsumer<ObtenerInformacionClienteYVendedor>
{
    public Task Consume(ConsumeContext<ObtenerInformacionClienteYVendedor> context)
    {
        var respuesta =
            usuariosDbContext.ObtenerInformacionClienteYVendedor(context.Message.IdCliente, context.Message.IdVendedor);
        
        return context.Publish(new InformacionClienteYVendedorObtenida(context.Message.IdProceso, respuesta.informacionCliente, respuesta.informacionVendedor));
    }
}