using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.Tests;

public class VentasRepositorioTests
{
    
    [Fact]
    public async Task GuardarPedidoAsync_DeberiaGuardarPedidoEnLaBaseDeDatos()
    {
        var options = new DbContextOptionsBuilder<VentasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using (var context = new VentasDbContext(options))
        {
            var pedido = new Pedido(1, new DateTime(2025, 4, 27),
            [
                    new ProductoPedido(101, 2, 50, false, null, null, null),
                    new ProductoPedido(102, 1, 100, false, null, null, null)
            ], "cliente 1", "direccion cliente 1", null, null);

            await context.GuardarPedidoAsync(pedido);

            var pedidoGuardado = await context.Pedidos
                .Include(p => p.ProductosPedidos)
                .FirstOrDefaultAsync(p => p.Id == 1);

            Assert.NotNull(pedidoGuardado);
            Assert.Equal(1, pedidoGuardado.IdCliente);
            Assert.Equal(1, pedidoGuardado.Id);
            Assert.Equal("cliente 1", pedidoGuardado.NombreCliente);
            Assert.Equal("direccion cliente 1", pedidoGuardado.DireccionEntrega);
            
            Assert.Equal(new DateTime(2025, 4, 27), pedidoGuardado.FechaCreacion);
            
            Assert.Equal(2, pedidoGuardado.ProductosPedidos.Count);
            Assert.Contains(pedidoGuardado.ProductosPedidos, pp => pp is {IdProducto: 101, Cantidad: 2, Precio: 50, IdPedido:1, TieneInventario: false});
            Assert.Contains(pedidoGuardado.ProductosPedidos, pp => pp is {IdProducto: 102, Cantidad: 1, Precio: 100, IdPedido:1, TieneInventario: false});
            
        }
    }
    
    [Fact]
    public async Task GuardarPedidoAsync_DeberiaGuardarDesdeSolicitudPedidoEnLaBaseDeDatos()
    {
        var options = new DbContextOptionsBuilder<VentasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using (var context = new VentasDbContext(options))
        {
            var solicitudPedido = new SolicitudDePedido(1, new DateTime(2025, 4, 27),
                [
                    new ProductoSolicitado(101, 2, 50, false),
                    new ProductoSolicitado(102, 1, 100, false)
                ]);
            List<InformacionPoducto> informacionProductos = [
                new(101, "imagen 101", "nombre 101", "codigo 101", 50)
            ];
            
            var pedido = new Pedido(solicitudPedido, informacionProductos, new InformacionCliente(1, "Direccion 1", "Nombre Cliente 1"), new InformacionVendedor(5, "Vendedor 5"));
            
            await context.GuardarPedidoAsync(pedido);

            var pedidoGuardado = await context.Pedidos
                .Include(p => p.ProductosPedidos)
                .FirstOrDefaultAsync(p => p.Id == 1);

            Assert.NotNull(pedidoGuardado);
            Assert.Equal(1, pedidoGuardado.IdCliente);
            Assert.Equal(1, pedidoGuardado.Id);
            Assert.Equal("Nombre Cliente 1", pedidoGuardado.NombreCliente);
            Assert.Equal("Direccion 1", pedidoGuardado.DireccionEntrega);
            Assert.Equal(5, pedidoGuardado.IdVendedor);
            Assert.Equal("Vendedor 5", pedidoGuardado.NombreVendedor);
            
            Assert.Equal(new DateTime(2025, 4, 27), pedidoGuardado.FechaCreacion);
            
            Assert.Equal(2, pedidoGuardado.ProductosPedidos.Count);
            Assert.Contains(pedidoGuardado.ProductosPedidos, pp => pp is {IdProducto: 101, Cantidad: 2, Precio: 50, IdPedido:1, TieneInventario: false, Codigo:"codigo 101", Nombre: "nombre 101", Imagen: "imagen 101"});
            Assert.Contains(pedidoGuardado.ProductosPedidos, pp => pp is {IdProducto: 102, Cantidad: 1, Precio: 100, IdPedido:1, TieneInventario: false, Codigo:"", Nombre:"", Imagen:""});
            
        }
    }
    
}