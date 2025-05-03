using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Entidades;
using StoreFlow.Compras.API.Errores.Productos;
using StoreFlow.Compras.API.Servicios;
using System.Text;

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


    public class ValidarProductosMasivoAsync
    {
        private ComprasDbContext CrearContextoInMemory(string dbName)
        {
            var opciones = new DbContextOptionsBuilder<ComprasDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var contexto = new ComprasDbContext(opciones);
            contexto.Fabricantes.AddRange(
                new Fabricante { Id = 1, RazonSocial = "Fabricante A", CorreoElectronico = "fa@correo.com" },
                new Fabricante { Id = 2, RazonSocial = "Fabricante B", CorreoElectronico = "fb@correo.com" },
                new Fabricante { Id = 3, RazonSocial = "Fabricante C", CorreoElectronico = "fc@correo.com" }
            );
            contexto.SaveChanges();
            return contexto;
        }

        private IFormFile CrearArchivoCsv(params string[] lineas)
        {
            var contenido = string.Join("\n", lineas);
            var bytes = Encoding.UTF8.GetBytes(contenido);
            return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "productos.csv");
        }

        [Fact]
        public async Task RetornaProductosSinErrores_CuandoArchivoEsValido()
        {
            var context = CrearContextoInMemory(nameof(RetornaProductosSinErrores_CuandoArchivoEsValido));
            var service = new ProductosService(context);
            var archivo = CrearArchivoCsv(
                "Producto1,SKU001,1,10000,https://example.com/img1.jpg",
                "Producto2,SKU002,2,20000,https://example.com/img2.jpg"
            );

            var resultado = await service.ValidarProductosMasivoAsync(archivo);

            Assert.Equal(2, resultado.Productos.Count);
            Assert.Empty(resultado.Errores);
        }

        [Fact]
        public async Task RetornaError_CuandoColumnasSonInsuficientes()
        {
            var context = CrearContextoInMemory(nameof(RetornaError_CuandoColumnasSonInsuficientes));
            var service = new ProductosService(context);
            var archivo = CrearArchivoCsv("Solo,3,campos");

            var resultado = await service.ValidarProductosMasivoAsync(archivo);

            Assert.Empty(resultado.Productos);
            Assert.Contains(resultado.Errores, e => e.Contains("columnas insuficientes"));
        }

        [Fact]
        public async Task RetornaError_CuandoNombreEsInvalido()
        {
            var context = CrearContextoInMemory(nameof(RetornaError_CuandoNombreEsInvalido));
            var service = new ProductosService(context);
            var archivo = CrearArchivoCsv(",SKU003,1,10000,https://example.com/img.jpg");

            var resultado = await service.ValidarProductosMasivoAsync(archivo);

            Assert.Empty(resultado.Productos);
            Assert.Contains("el nombre está vacío", resultado.Errores[0]);
        }

        [Fact]
        public async Task RetornaError_CuandoSkuEsDuplicadoEnElArchivo()
        {
            var context = CrearContextoInMemory(nameof(RetornaError_CuandoSkuEsDuplicadoEnElArchivo));
            var service = new ProductosService(context);
            var archivo = CrearArchivoCsv(
                "Producto1,SKUX,1,10000,https://example.com/1.jpg",
                "Producto2,SKUX,1,20000,https://example.com/2.jpg"
            );

            var resultado = await service.ValidarProductosMasivoAsync(archivo);

            Assert.Equal(2, resultado.Errores.Count);
            Assert.All(resultado.Errores, e => Assert.Contains("duplicado en el archivo", e));
        }

        [Fact]
        public async Task RetornaError_CuandoFabricanteNoExiste()
        {
            var context = CrearContextoInMemory(nameof(RetornaError_CuandoFabricanteNoExiste));
            var service = new ProductosService(context);
            var archivo = CrearArchivoCsv("Producto1,SKU004,999,10000,https://example.com/img.jpg");

            var resultado = await service.ValidarProductosMasivoAsync(archivo);

            Assert.Empty(resultado.Productos);
            Assert.Contains("el fabricante no existe", resultado.Errores[0]);
        }

        [Fact]
        public async Task RetornaError_CuandoPrecioEsInvalido()
        {
            var context = CrearContextoInMemory(nameof(RetornaError_CuandoPrecioEsInvalido));
            var service = new ProductosService(context);
            var archivo = CrearArchivoCsv("Producto1,SKU005,1,-100,https://example.com/img.jpg");

            var resultado = await service.ValidarProductosMasivoAsync(archivo);

            Assert.Empty(resultado.Productos);
            Assert.Contains("el precio es inválido", resultado.Errores[0]);
        }

        [Fact]
        public async Task RetornaError_CuandoImagenEsInvalida()
        {
            var context = CrearContextoInMemory(nameof(RetornaError_CuandoImagenEsInvalida));
            var service = new ProductosService(context);
            var archivo = CrearArchivoCsv("Producto1,SKU006,1,15000,not-a-url");

            var resultado = await service.ValidarProductosMasivoAsync(archivo);

            Assert.Empty(resultado.Productos);
            Assert.Contains("la URL de la imagen no es válida", resultado.Errores[0]);
        }
    }

    public class GuardarProductosMasivosAsync
    {
        private ComprasDbContext CrearContextoInMemory(string dbName)
        {
            var opciones = new DbContextOptionsBuilder<ComprasDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var contexto = new ComprasDbContext(opciones);
            contexto.Fabricantes.AddRange(
                new Fabricante { Id = 1, RazonSocial = "Fabricante A", CorreoElectronico = "fa@correo.com" },
                new Fabricante { Id = 2, RazonSocial = "Fabricante B", CorreoElectronico = "fb@correo.com" }
            );
            contexto.SaveChanges();
            return contexto;
        }

        [Fact]
        public async Task GuardaCorrectamente_CuandoProductosSonValidos()
        {
            var context = CrearContextoInMemory(nameof(GuardaCorrectamente_CuandoProductosSonValidos));
            var service = new ProductosService(context);

            var productos = new List<RegistrarProducto>
            {
                new("Producto A", "SKU_A", 15000, "https://example.com/a.jpg", 1),
                new("Producto B", "SKU_B", 20000, "https://example.com/b.jpg", 2)
            };

            await service.GuardarProductosMasivosAsync(productos);

            var guardados = await context.Productos.ToListAsync();
            Assert.Equal(2, guardados.Count);
            Assert.Contains(guardados, p => p.Sku == "SKU_A");
            Assert.Contains(guardados, p => p.Sku == "SKU_B");
        }

        [Fact]
        public async Task NoGuardaNada_CuandoSkusYaExisten()
        {
            var context = CrearContextoInMemory(nameof(NoGuardaNada_CuandoSkusYaExisten));
            context.Productos.Add(new Producto
            { Nombre = "Existente", Sku = "SKU_EXISTE", Precio = 10000, ImagenUrl = "url", FabricanteId = 1 });
            context.SaveChanges();

            var service = new ProductosService(context);
            var productos = new List<RegistrarProducto>
            {
                new("Repetido", "SKU_EXISTE", 12000, "https://example.com/rep.jpg", 1)
            };

            await service.GuardarProductosMasivosAsync(productos);

            var guardados = await context.Productos.ToListAsync();
            Assert.Single(guardados); // solo el original
        }

        [Fact]
        public async Task GuardaSoloLosNuevos_CuandoAlgunosSkusYaExisten()
        {
            var context = CrearContextoInMemory(nameof(GuardaSoloLosNuevos_CuandoAlgunosSkusYaExisten));
            context.Productos.Add(new Producto
            { Nombre = "YaExiste", Sku = "SKU_EXISTE", Precio = 12000, ImagenUrl = "url", FabricanteId = 1 });
            context.SaveChanges();

            var service = new ProductosService(context);
            var productos = new List<RegistrarProducto>
            {
                new("Producto Nuevo", "SKU_NUEVO", 18000, "https://example.com/new.jpg", 2),
                new("Repetido", "SKU_EXISTE", 13000, "https://example.com/rep.jpg", 1)
            };

            await service.GuardarProductosMasivosAsync(productos);

            var guardados = await context.Productos.ToListAsync();
            Assert.Equal(2, guardados.Count); // el que ya existía + el nuevo
            Assert.Contains(guardados, p => p.Sku == "SKU_NUEVO");
        }
    }
}