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
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
        Assert.Equal(1, entrega.ProductosPedidos[0].IdEntrega);
        Assert.Equal(1, entrega.ProductosPedidos[0].IdProducto);
        Assert.Equal(10, entrega.ProductosPedidos[0].Cantidad);
        Assert.Equal(100_000, entrega.ProductosPedidos[0].Precio);
        Assert.Equal("P001", entrega.ProductosPedidos[0].Codigo);
        Assert.Equal("Producto 1", entrega.ProductosPedidos[0].Nombre);
        Assert.Equal("imagen1.jpg", entrega.ProductosPedidos[0].Imagen);
        
    }

    [Fact]
    public async Task ObtenerEntregasClienteAsync_RetornaEntregasCorrectamente()
    {
        var options = new DbContextOptionsBuilder<LogisticaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var contexto = new LogisticaDbContext(options);
        var servicio = new EntregaServicio(contexto);

        var pedido1 = new PedidoResponse(100, 1, new DateTime(2025, 5, 3), new DateTime(2025, 5, 6),
            "Dirección Cliente 1",
            EstadoPedido.Pendiente, [
                new ProductoPedidoResponse(1001, 10, 100_000, "P001", "Producto 1", "imagen1.jpg")
            ], 1_000_000, "Cliente 1");

        var pedido2 = new PedidoResponse(101, 1, new DateTime(2025, 5, 4), new DateTime(2025, 5, 7),
            "Dirección Cliente 1",
            EstadoPedido.Pendiente, [
                new ProductoPedidoResponse(2, 5, 50_000, "P002", "Producto 2", "imagen2.jpg")
            ], 500_000, "Cliente 1");
        var pedido3 = new PedidoResponse(102, 2, new DateTime(2025, 5, 4), new DateTime(2025, 5, 7),
            "Dirección Cliente 2",
            EstadoPedido.Pendiente, [
                new ProductoPedidoResponse(3, 5, 50_000, "P002", "Producto 2", "imagen2.jpg")
            ], 500_000, "Cliente 2");

        await servicio.GuardarEntregaAsync(pedido1);
        await servicio.GuardarEntregaAsync(pedido2);
        await servicio.GuardarEntregaAsync(pedido3);

        var entregas = await servicio.ObtenerEntregasClienteAsync(1);

        Assert.Equal(2, entregas.Length);

        Assert.Equal(100, entregas[0].Numero);
        Assert.Equal(1, entregas[0].Id);
        Assert.Equal(new DateTime(2025, 5, 6), entregas[0].FechaEntrega);
        Assert.Equal("Dirección Cliente 1", entregas[0].LugarEntrega);
        Assert.Single(entregas[0].Productos);
        Assert.Equal(1001, entregas[0].Productos[0].Id);
        Assert.Equal(10, entregas[0].Productos[0].Cantidad);
        Assert.Equal(100_000, entregas[0].Productos[0].Precio);
        Assert.Equal("P001", entregas[0].Productos[0].Codigo);
        Assert.Equal("Producto 1", entregas[0].Productos[0].Nombre);
        Assert.Equal("imagen1.jpg", entregas[0].Productos[0].Imagen);
        Assert.Equal(101, entregas[1].Numero);
    }



}