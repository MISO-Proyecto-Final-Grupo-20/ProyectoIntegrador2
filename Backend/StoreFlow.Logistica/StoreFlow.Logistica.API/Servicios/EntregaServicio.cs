using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Logistica.API.Datos;
using StoreFlow.Logistica.API.Entidades;

namespace StoreFlow.Logistica.API.Servicios;

public interface IEntregaServicio
{
    Task GuardarEntregaAsync( PedidoResponse pedido);
}

public class EntregaServicio : IEntregaServicio
{
    private readonly LogisticaDbContext _contexto;

    public EntregaServicio(LogisticaDbContext contexto)
    {
        _contexto = contexto;
    }    
    
    public async Task GuardarEntregaAsync( PedidoResponse pedido)
    {
        var entrega = new Entrega(pedido);
        await _contexto.Entregas.AddAsync(entrega);
        await _contexto.SaveChangesAsync();
    }
    
}