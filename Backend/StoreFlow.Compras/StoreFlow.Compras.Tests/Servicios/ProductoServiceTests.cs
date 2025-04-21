using Microsoft.EntityFrameworkCore;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Entidades;
using StoreFlow.Compras.API.Errores.Productos;
using StoreFlow.Compras.API.Servicios;

namespace StoreFlow.Compras.Tests.Servicios;

public class ProductoServiceTests
{
    [Fact]
    public async Task DebeCrearProducto_CuandoElSkuNoExiste()
    {
        // Arrange
        var opciones = new DbContextOptionsBuilder<ComprasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var contexto = new ComprasDbContext(opciones);
        contexto.Fabricantes.Add(new Fabricante
        { Id = 1, RazonSocial = "Sony", CorreoElectronico = "sony@correo.com" });
        await contexto.SaveChangesAsync();

        var servicio = new ProductosService(contexto);

        var request = new CrearProductoRequest(
            Nombre: "PlayStation 5",
            FabricanteAsociado: 1,
            Codigo: "PS5-001",
            Precio: 2500000,
            Imagen: "https://ejemplo.com/ps5.jpg"
        );

        // Act
        var resultado = await servicio.CrearProductoAsync(request);

        // Assert
        Assert.True(resultado.EsExitoso);
        Assert.NotNull(resultado.Valor);
        Assert.Equal("PS5-001", resultado.Valor!.Sku);
        Assert.Equal("PlayStation 5", resultado.Valor.Nombre);
    }

    [Fact]
    public async Task DebeFallar_CuandoElSkuYaExiste()
    {
        // Arrange
        var opciones = new DbContextOptionsBuilder<ComprasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var contexto = new ComprasDbContext(opciones);

        contexto.Fabricantes.Add(new Fabricante
        { Id = 1, RazonSocial = "Nintendo", CorreoElectronico = "nintendo@correo.com" });
        contexto.Productos.Add(new Producto
        {
            Nombre = "Switch",
            Sku = "SW-001",
            Precio = 1500000,
            ImagenUrl = "https://ejemplo.com/switch.jpg",
            FabricanteId = 1
        });

        await contexto.SaveChangesAsync();

        var servicio = new ProductosService(contexto);

        var request = new CrearProductoRequest(
            Nombre: "Nintendo Switch OLED",
            FabricanteAsociado: 1,
            Codigo: "SW-001", // SKU duplicado
            Precio: 1800000,
            Imagen: "https://ejemplo.com/switch-oled.jpg"
        );

        // Act
        var resultado = await servicio.CrearProductoAsync(request);

        // Assert
        Assert.False(resultado.EsExitoso);
        Assert.Null(resultado.Valor);
        Assert.IsType<SkuYaExiste>(resultado.Error);
    }
}