using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Inventarios.API.Datos;
using StoreFlow.Inventarios.API.Entidades;

namespace StoreFlow.Inventarios.Tests;

public class InventarioRepositorioTest
{
    [Fact]
    public async Task ExisteInventariosuficiente_DeberiaRetornarProductosConEstadoInventario()
    {
        var options = new DbContextOptionsBuilder<InventariosDbContext>()
            .UseInMemoryDatabase("InventariosTestDb")
            .Options;

        await using (var context = new InventariosDbContext(options))
        {
            context.Inventarios.AddRange(
                new Inventario(1, 1, 10),
                new Inventario(2, 1, 5)
            );
            await context.SaveChangesAsync();
        }

        await using (var context = new InventariosDbContext(options))
        {
            var pedido = new SolicitudDePedido(
                3,
                new DateTime(2025, 4, 27),
                [
                    new ProductoSolicitado(1, 5, 100, false),
                    new ProductoSolicitado(2, 10, 50, false),
                    new ProductoSolicitado(3, 1, 20, false)
                ], null
            );

            var pedidoValidado = await context.ValidarPedidoConInventarioAsync(pedido);

            Assert.Equal(pedidoValidado.FechaCreacion, pedido.FechaCreacion);
            Assert.Equal(pedidoValidado.IdCliente, pedido.IdCliente);
            Assert.True(pedidoValidado.ProductosSolicitados[0].TieneInventario);
            Assert.False(pedidoValidado.ProductosSolicitados[1].TieneInventario);
            Assert.False(pedidoValidado.ProductosSolicitados[2].TieneInventario);


            var inventarioActualizado = await context.Inventarios.ToListAsync();

            Assert.Equal(5, inventarioActualizado.First(i => i.IdProducto == 1).Cantidad);
            Assert.Equal(5, inventarioActualizado.First(i => i.IdProducto == 2).Cantidad);
            Assert.DoesNotContain(inventarioActualizado, i => i.IdProducto == 3);
        }
    }

    [Theory]
    [InlineData(1, 10, 5, true)]
    [InlineData(2, 5, 10, false)]
    [InlineData(3, 0, 1, false)]
    public async Task ExisteInventariosuficiente_DeberiaRetornarEstadoCorrecto(
        int idProducto, int cantidadInicial, int cantidadSolicitada, bool tieneInventarioEsperado)
    {
        var options = new DbContextOptionsBuilder<InventariosDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using (var context = new InventariosDbContext(options))
        {
            context.Inventarios.Add(new Inventario(idProducto, 1, cantidadInicial));
            await context.SaveChangesAsync();
        }

        await using (var context = new InventariosDbContext(options))
        {
            var resultado = await context.ExisteInventarioSuficienteAsync(idProducto, cantidadSolicitada);

            Assert.Equal(tieneInventarioEsperado, resultado);
        }
    }
}