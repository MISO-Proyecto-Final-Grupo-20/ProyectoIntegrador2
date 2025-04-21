using StoreFlow.Ventas.API.EndPoints;

namespace StoreFlow.Ventas.API.Servicios;

public interface IPedidoService
{
    Task<Resultado<CrearPedidoResponse>> CrearPedidoAsync(CrearPedidoCommand crearPedido);
}