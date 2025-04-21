using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Entidades;
using StoreFlow.Compras.Tests.Utilidades;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StoreFlow.Compras.Tests.Endpoints
{
    public class FabricantesEndpointTests : IAsyncLifetime
    {
        private HttpClient _client = null!;
        private WebApplication _app = null!;

        public async Task InitializeAsync()
        {
            _app = TestApplicationFactory.Create();
            await _app.StartAsync();
            _client = _app.GetTestClient();
        }

        [Fact]
        public async Task DebeRetornarCreated_CuandoLaSolicitudEsValida()
        {
            var request = new CrearFabricanteRequest("ACME", "valido@correo.com");
            var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await _client.PostAsJsonAsync("/fabricantes", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task DebeRetornarConflict_CuandoElCorreoYaExiste()
        {
            var request = new CrearFabricanteRequest("Empresa", "duplicado@correo.com");
            var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            await _client.PostAsJsonAsync("/fabricantes", request); // Primera vez
            var response = await _client.PostAsJsonAsync("/fabricantes", request); // Duplicado

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DebeRetornarValidationProblem_CuandoElNombreEsNulo()
        {
            var request = new { Nombre = (string?)null, CorreoElectronico = "correo@dominio.com" };
            var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await _client.PostAsJsonAsync("/fabricantes", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DebeRetornarValidationProblem_CuandoElCorreoEsNulo()
        {
            var request = new { Nombre = "Empresa", CorreoElectronico = (string?)null };
            var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await _client.PostAsJsonAsync("/fabricantes", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DebeRetornarValidationProblem_CuandoElCorreoTieneFormatoInvalido()
        {
            var request = new CrearFabricanteRequest("Empresa", "formato_invalido");
            var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await _client.PostAsJsonAsync("/fabricantes", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DebeRetornarValidationProblem_CuandoElNombreExcedeLongitud()
        {
            var nombreLargo = new string('X', 101);
            var request = new CrearFabricanteRequest(nombreLargo, "correo@empresa.com");
            var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await _client.PostAsJsonAsync("/fabricantes", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DebeRetornarValidationProblem_CuandoElCorreoExcedeLongitud()
        {
            var correoLargo = new string('a', 101) + "@dominio.com";
            var request = new CrearFabricanteRequest("Empresa", correoLargo);
            var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await _client.PostAsJsonAsync("/fabricantes", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DebeRetornarUnauthorized_CuandoNoHayToken()
        {
            var request = new CrearFabricanteRequest("Empresa", "sin-token@empresa.com");

            _client.DefaultRequestHeaders.Authorization = null;

            var response = await _client.PostAsJsonAsync("/fabricantes", request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DebeRetornarForbidden_CuandoElRolNoEsUsuarioCcp()
        {
            var request = new CrearFabricanteRequest("Empresa", "noadmin@empresa.com");

            var jwt = GeneradorTokenPruebas.GenerarTokenVendedor();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await _client.PostAsJsonAsync("/fabricantes", request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }


        [Fact]
        public async Task GetFabricantes_DebeRetornar200YListado_CuandoTokenEsValidoYHayFabricantes()
        {
            var dbName = $"db-test-{Guid.NewGuid()}";
            var app = TestApplicationFactory.Create(dbName);
            await app.StartAsync();
            var client = app.GetTestClient();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ComprasDbContext>();
                db.Fabricantes.AddRange(
                    new Fabricante { RazonSocial = "Sony", CorreoElectronico = "sony@empresa.com" },
                    new Fabricante { RazonSocial = "HP", CorreoElectronico = "hp@empresa.com" }
                );
                await db.SaveChangesAsync();
            }

            var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await client.GetAsync("/fabricantes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var resultado = await response.Content.ReadFromJsonAsync<List<FabricanteDto>>();
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Contains(resultado, f => f.Nombre == "Sony");
            Assert.Contains(resultado, f => f.Nombre == "HP");

            await app.StopAsync();
        }

        [Fact]
        public async Task GetFabricantes_DebeRetornar200YListaVacia_CuandoNoHayFabricantes()
        {
            var dbName = $"db-test-{Guid.NewGuid()}";
            var app = TestApplicationFactory.Create(dbName);
            await app.StartAsync();
            var client = app.GetTestClient();

            var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await client.GetAsync("/fabricantes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var resultado = await response.Content.ReadFromJsonAsync<List<FabricanteDto>>();
            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            await app.StopAsync();
        }

        [Fact]
        public async Task GetFabricantes_DebeRetornarUnauthorized_CuandoNoHayToken()
        {
            var dbName = $"db-test-{Guid.NewGuid()}";
            var app = TestApplicationFactory.Create(dbName);
            await app.StartAsync();
            var client = app.GetTestClient();

            client.DefaultRequestHeaders.Authorization = null;

            var response = await client.GetAsync("/fabricantes");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            await app.StopAsync();
        }

        [Fact]
        public async Task GetFabricantes_DebeRetornarForbidden_CuandoElRolEsIncorrecto()
        {
            var dbName = $"db-test-{Guid.NewGuid()}";
            var app = TestApplicationFactory.Create(dbName);
            await app.StartAsync();
            var client = app.GetTestClient();

            var jwt = GeneradorTokenPruebas.GenerarTokenVendedor(); // rol incorrecto
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await client.GetAsync("/fabricantes");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            await app.StopAsync();
        }


        public async Task DisposeAsync()
        {
            await _app.StopAsync();
        }
    }
}