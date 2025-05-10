using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Logistica.API.Datos;
using StoreFlow.Logistica.API.Servicios;

namespace StoreFlow.Logistica.Tests.Utilidades;

public class EntregaServicioTest
{
    [Fact]
    public async Task GuardarEntregaAsync_AlmacenaEntregaCorrectamente()
    {
        var options = new DbContextOptionsBuilder<LogisticaDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var contexto = new LogisticaDbContext(options);
        var servicio = new EntregaServicio(contexto);

        var pedido = new PedidoResponse(100, 1, new DateTime(2025, 5, 3), new DateTime(2025, 5, 6), "Dirección Cliente 1",
            EstadoPedido.Pendiente, new[]
            {
                new ProductoPedidoResponse(1, 10, 100_000, "P001", "Producto 1", "imagen1.jpg"),
            }, 1_000_000, "Cliente 1");
            

        await servicio.GuardarEntregaAsync(pedido);

        var entrega = await contexto.Entregas
            .Include(entrega => entrega.ProductosPedidos)
            .FirstOrDefaultAsync();
        
        Assert.NotNull(entrega);
        Assert.Equal(1, entrega.Id);
        Assert.Equal(100, entrega.IdPedido);
        Assert.Equal(1, entrega.IdCliente);
        Assert.Equal("Cliente 1", entrega.NombreCliente);
        Assert.Equal("Dirección Cliente 1", entrega.DireccionEntrega);
        Assert.Equal(new DateTime(2025, 5, 3), entrega.FechaCreacion);
        Assert.Equal(new DateTime(2025, 5, 6), entrega.FechaProgramadaEntrega);
        Assert.Single(entrega.ProductosPedidos);
        Assert.Equal(1, entrega.ProductosPedidos[0].IdProducto);
        Assert.Equal(10, entrega.ProductosPedidos[0].Cantidad);
        Assert.Equal(100_000, entrega.ProductosPedidos[0].Precio);
        Assert.Equal("P001", entrega.ProductosPedidos[0].Codigo);
        Assert.Equal("Producto 1", entrega.ProductosPedidos[0].Nombre);
        Assert.Equal("imagen1.jpg", entrega.ProductosPedidos[0].Imagen);
        
    }
}