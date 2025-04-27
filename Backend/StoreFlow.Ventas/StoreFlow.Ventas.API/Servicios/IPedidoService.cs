using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Ventas.API.EndPoints;

namespace StoreFlow.Ventas.API.Servicios;

public interface IPedidoService
{
    Task<Resultado<CrearPedidoResponse>> CrearPedidoAsync(SolicitudDePedido solicitudDePedido);
}