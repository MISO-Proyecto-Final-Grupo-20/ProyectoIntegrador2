using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Logistica.API.Datos;
using StoreFlow.Logistica.API.DTOs;
using StoreFlow.Logistica.API.Entidades;

namespace StoreFlow.Logistica.API.Servicios;

public interface IEntregaServicio
{
    Task GuardarEntregaAsync(PedidoResponse pedido);
    Task<EntregaProgramadaResponse[]> ObtenerEntregasClienteAsync(int idCliente);
}

public class EntregaServicio(LogisticaDbContext contexto) : IEntregaServicio
{
    public async Task GuardarEntregaAsync(PedidoResponse pedido)
    {
        var entrega = new Entrega(pedido);
        await contexto.Entregas.AddAsync(entrega);
        await contexto.SaveChangesAsync();
    }

    public async Task<EntregaProgramadaResponse[]> ObtenerEntregasClienteAsync(int idCliente)
    {
        return await contexto
            .Entregas
            .Include(e => e.ProductosPedidos)
            .Where(e => e.IdCliente == idCliente)
            .Select(e => new EntregaProgramadaResponse(e.Id, e.IdPedido, e.FechaProgramadaEntrega, e.DireccionEntrega,
                e.ProductosPedidos.Select(p =>
                    new ProductoPedidoResponse(p.IdProducto, p.Cantidad, p.Precio, p.Codigo, p.Nombre, p.Imagen)
                ).ToArray()
            ))
            .ToArrayAsync();
    }
}