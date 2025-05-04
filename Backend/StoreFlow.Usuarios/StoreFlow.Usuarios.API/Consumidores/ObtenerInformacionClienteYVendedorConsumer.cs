using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.Servicios;

namespace StoreFlow.Usuarios.API.Consumidores;

public class ObtenerInformacionClienteYVendedorConsumer(IUsuariosServicios usuariosServicios) : IConsumer<ObtenerInformacionClienteYVendedor>
{
    public Task Consume(ConsumeContext<ObtenerInformacionClienteYVendedor> context)
    {
        var respuesta =
            usuariosServicios.ObtenerInformacionClienteYVendedor(context.Message.IdCliente, context.Message.IdVendedor);
        
        return context.Publish(new InformacionClienteYVendedorObtenida(context.Message.IdProceso, respuesta.informacionCliente, respuesta.informacionVendedor));
    }
}