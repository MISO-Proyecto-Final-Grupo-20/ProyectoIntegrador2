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
            .UseInMemoryDatabase(databaseName: "InventariosTestDb")
            .Options;

        await using (var context = new InventariosDbContext(options))
        {
            context.Inventarios.AddRange(
                new Inventario(1, 10),
                new Inventario(2, 5)
            );
            await context.SaveChangesAsync();
        }

        await using (var context = new InventariosDbContext(options))
        {
            // Crear el pedido de prueba
            var pedido = new SolicitudDePedido(
                3,
                new DateTime(2025, 4, 27),
                new[]
                {
                    new ProductoSolicitado(Id: 1, Cantidad: 5, Precio: 100, TieneInventario: false),
                    new ProductoSolicitado(Id: 2, Cantidad: 10, Precio: 50, TieneInventario: false),
                    new ProductoSolicitado(Id: 3, Cantidad: 1, Precio: 20, TieneInventario: false)
                }
            );

            var resultado = await context.ValidarPedidoConInventarioAsync(pedido);

            Assert.Equal(resultado.FechaCreacion, pedido.FechaCreacion);
            Assert.Equal(resultado.IdCliente, pedido.IdCliente);
            Assert.True(resultado.productosSolicitados[0].TieneInventario); 
            Assert.False(resultado.productosSolicitados[1].TieneInventario);
            Assert.False(resultado.productosSolicitados[2].TieneInventario); 
        }
    }
}