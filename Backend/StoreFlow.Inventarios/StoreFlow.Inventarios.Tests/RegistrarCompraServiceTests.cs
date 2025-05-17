using Microsoft.EntityFrameworkCore;
using StoreFlow.Inventarios.API.Datos;
using StoreFlow.Inventarios.API.DTOs;
using StoreFlow.Inventarios.API.Entidades;
using StoreFlow.Inventarios.API.Servicios;

namespace StoreFlow.Inventarios.Tests;

public class RegistrarCompraServiceTests
{
    private static InventariosDbContext CrearContextoEnMemoria(string nombre)
    {
        var options = new DbContextOptionsBuilder<InventariosDbContext>()
            .UseInMemoryDatabase(nombre)
            .Options;
        return new InventariosDbContext(options);
    }

    [Fact]
    public async Task DebeLanzarExcepcion_SiBodegaNoExiste()
    {
        var db = CrearContextoEnMemoria("sin_bodega");
        var servicio = new RegistrarCompraService(db);

        var dto = new RegistroCompraBodegaDto
        {
            Bodega = 99,
            Fabricante = 1,
            Productos = new List<ProductoAComprarDto> { new(1, 5) }
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => servicio.RegistrarCompraAsync(dto));
    }

    [Fact]
    public async Task DebeCrearInventario_SiNoExiste()
    {
        var db = CrearContextoEnMemoria("crear_inventario");
        db.Bodegas.Add(new Bodega(1, "Test"));
        await db.SaveChangesAsync();

        var servicio = new RegistrarCompraService(db);

        var dto = new RegistroCompraBodegaDto
        {
            Bodega = 1,
            Fabricante = 1,
            Productos = new List<ProductoAComprarDto> { new(10, 3) }
        };

        await servicio.RegistrarCompraAsync(dto);

        var inventario = await db.Inventarios.FindAsync(10, 1);
        Assert.NotNull(inventario);
        Assert.Equal(3, inventario!.Cantidad);
    }

    [Fact]
    public async Task DebeSumarCantidad_SiInventarioExiste()
    {
        var db = CrearContextoEnMemoria("sumar_inventario");
        db.Bodegas.Add(new Bodega(1, "Test"));
        db.Inventarios.Add(new Inventario(10, 1, 5));
        await db.SaveChangesAsync();

        var servicio = new RegistrarCompraService(db);

        var dto = new RegistroCompraBodegaDto
        {
            Bodega = 1,
            Fabricante = 1,
            Productos = new List<ProductoAComprarDto> { new(10, 4) }
        };

        await servicio.RegistrarCompraAsync(dto);

        var inventario = await db.Inventarios.FindAsync(10, 1);
        Assert.NotNull(inventario);
        Assert.Equal(9, inventario!.Cantidad);
    }
}