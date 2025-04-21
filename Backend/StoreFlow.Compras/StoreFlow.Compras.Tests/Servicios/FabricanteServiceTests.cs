using Microsoft.EntityFrameworkCore;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Entidades;
using StoreFlow.Compras.API.Errores.Fabricantes;
using StoreFlow.Compras.API.Servicios;

namespace StoreFlow.Compras.Tests.Servicios
{
    public class FabricanteServiceTests
    {
        [Fact]
        public async Task DebeCrearFabricante_CuandoElCorreoNoExiste()
        {
            // Arrange
            var opciones = new DbContextOptionsBuilder<ComprasDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var contexto = new ComprasDbContext(opciones);
            var servicio = new FabricantesService(contexto);

            var request = new CrearFabricanteRequest("ACME", "acme@empresa.com");

            // Act
            var resultado = await servicio.CrearFabricanteAsync(request);

            // Assert
            Assert.True(resultado.EsExitoso);
            Assert.NotNull(resultado.Valor);
            Assert.Equal("ACME", resultado.Valor!.Nombre);
            Assert.Equal("acme@empresa.com", resultado.Valor.Correo);
        }

        [Fact]
        public async Task DebeFallar_CuandoElCorreoYaExiste()
        {
            // Arrange
            var opciones = new DbContextOptionsBuilder<ComprasDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var contexto = new ComprasDbContext(opciones);
            contexto.Fabricantes.Add(new Fabricante
            {
                RazonSocial = "Duplicado",
                CorreoElectronico = "existe@empresa.com"
            });
            await contexto.SaveChangesAsync();

            var servicio = new FabricantesService(contexto);
            var request = new CrearFabricanteRequest("Otra Empresa", "existe@empresa.com");

            // Act
            var resultado = await servicio.CrearFabricanteAsync(request);

            // Assert
            Assert.False(resultado.EsExitoso);
            Assert.Null(resultado.Valor);
            Assert.IsType<FabricanteYaExiste>(resultado.Error);
        }

        [Fact]
        public async Task ObtenerListadoAsync_DebeRetornarFabricantesOrdenadosPorNombre()
        {
            // Arrange
            var dbName = $"db-test-{Guid.NewGuid()}";
            using var contexto = CrearContexto(dbName);

            contexto.Fabricantes.AddRange(
                new Fabricante { RazonSocial = "Zebra", CorreoElectronico = "zebra@empresa.com" },
                new Fabricante { RazonSocial = "Alfa", CorreoElectronico = "alfa@empresa.com" },
                new Fabricante { RazonSocial = "Beta", CorreoElectronico = "beta@empresa.com" }
            );
            await contexto.SaveChangesAsync();

            var servicio = new FabricantesService(contexto);

            // Act
            var resultado = await servicio.ObtenerListadoAsync();

            // Assert
            Assert.Equal(3, resultado.Count);
            Assert.Collection(resultado,
                f => Assert.Equal("Alfa", f.Nombre),
                f => Assert.Equal("Beta", f.Nombre),
                f => Assert.Equal("Zebra", f.Nombre)
            );
        }

        [Fact]
        public async Task ObtenerListadoAsync_DebeRetornarListaVacia_CuandoNoHayFabricantes()
        {
            // Arrange
            var dbName = $"db-test-{Guid.NewGuid()}";
            using var contexto = CrearContexto(dbName);
            var servicio = new FabricantesService(contexto);

            // Act
            var resultado = await servicio.ObtenerListadoAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }


        private static ComprasDbContext CrearContexto(string dbName)
        {
            var options = new DbContextOptionsBuilder<ComprasDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ComprasDbContext(options);
        }
    }
}