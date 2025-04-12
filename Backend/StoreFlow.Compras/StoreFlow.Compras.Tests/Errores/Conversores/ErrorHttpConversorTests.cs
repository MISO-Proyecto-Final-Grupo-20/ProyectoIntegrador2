using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Compras.API.Errores;
using StoreFlow.Compras.API.Errores.Conversores;
using StoreFlow.Compras.API.Errores.Fabricantes;

namespace StoreFlow.Compras.Tests.Errores.Conversores
{
    public class ErrorHttpConversorTests
    {
        [Fact]
        public async Task DebeRetornarConflict_CuandoElErrorEsConflictoNegocio()
        {
            // Arrange
            var error = new FabricanteYaExiste("correo@ejemplo.com");
            var resultado = ErrorHttpConversor.Convertir(error);

            var services = new ServiceCollection();
            services.AddRouting();
            services.AddLogging();
            services.AddControllers();
            var serviceProvider = services.BuildServiceProvider();

            var context = new DefaultHttpContext();
            context.RequestServices = serviceProvider;
            context.Response.Body = new MemoryStream();

            // Act
            await resultado.ExecuteAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);

            // Validar contenido opcional
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains("correo@ejemplo.com", body);
        }

        [Fact]
        public async Task DebeRetornarBadRequest_CuandoLaCategoriaNoEsReconocida()
        {
            // Arrange
            var error = new ErrorEntradaInvalida("Este error no tiene categoría conocida");
            var resultado = ErrorHttpConversor.Convertir(error);

            var services = new ServiceCollection();
            services.AddRouting();
            services.AddLogging();
            services.AddControllers();
            var serviceProvider = services.BuildServiceProvider();

            var context = new DefaultHttpContext();
            context.RequestServices = serviceProvider;
            context.Response.Body = new MemoryStream();

            // Act
            await resultado.ExecuteAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);

            // Validar mensaje en el body
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains("Este error no tiene categoría conocida", body);
        }
    }

    internal class ErrorEntradaInvalida : ErrorDeNegocio
    {
        private readonly string _mensaje;

        public ErrorEntradaInvalida(string mensaje) => _mensaje = mensaje;

        public override string Mensaje => _mensaje;
        public override string Categoria => "ENTRADA_INVALIDA";
    }
}