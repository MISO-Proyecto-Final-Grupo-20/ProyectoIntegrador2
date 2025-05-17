using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Inventarios.API.Datos;
using StoreFlow.Inventarios.API.DTOs;
using StoreFlow.Inventarios.API.Entidades;

namespace StoreFlow.Inventarios.Tests;

public class ComprasEndpointTests : IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebApplication _app = null!;

    public async Task InitializeAsync()
    {
        var nombreDb = Guid.NewGuid().ToString();
        _app = TestApplicationFactory.Create(nombreDb);
        await _app.StartAsync();
        _client = _app.GetTestClient();
    }

    [Fact]
    public async Task ObtenerBodegas_DebeRetornarOk_CuandoTokenEsValido()
    {
        AutenticarComoCcp();

        var response = await _client.GetAsync("/bodegas");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var bodegas = await response.Content.ReadFromJsonAsync<List<BodegaResponse>>();
        Assert.NotNull(bodegas);
        Assert.All(bodegas, b =>
        {
            Assert.True(b.id > 0);
            Assert.False(string.IsNullOrWhiteSpace(b.descripcion));
        });
    }

    [Fact]
    public async Task ObtenerBodegas_DebeRetornar401_SiNoHayToken()
    {
        SinAutenticacion();

        var response = await _client.GetAsync("/bodegas");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ObtenerBodegas_DebeRetornar403_SiRolEsIncorrecto()
    {
        AutenticarComoCliente();

        var response = await _client.GetAsync("/bodegas");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task RegistrarCompra_DebeRetornarOk_CuandoSolicitudEsValida()
    {
        var app = TestApplicationFactory.Create(Guid.NewGuid().ToString());
        await app.StartAsync();
        var client = app.GetTestClient();

        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InventariosDbContext>();
        db.Bodegas.Add(new Bodega(1, "Bodega Test"));
        db.Inventarios.Add(new Inventario(1, 1, 50));
        db.Inventarios.Add(new Inventario(2, 1, 50));
        await db.SaveChangesAsync();

        var payload = new RegistroCompraBodegaDto
        {
            Bodega = 1,
            Fabricante = 1,
            Productos = new List<ProductoAComprarDto>
            {
                new(1, 3),
                new(2, 2)
            }
        };

        var response = await client.PostAsJsonAsync("/compras", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultado = await response.Content.ReadFromJsonAsync<CompraRegistradaResponse>();
        Assert.NotNull(resultado);
        Assert.Equal("Compra registrada exitosamente.", resultado!.mensaje);
        Assert.Equal(5, resultado.totalProductos);

        await app.StopAsync();
    }

    [Fact]
    public async Task RegistrarCompra_DebeRetornar400_CuandoProductosEstanVacios()
    {
        var app = TestApplicationFactory.Create(Guid.NewGuid().ToString());
        await app.StartAsync();
        var client = app.GetTestClient();

        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InventariosDbContext>();
        db.Bodegas.Add(new Bodega(1, "Bodega Test"));
        await db.SaveChangesAsync();

        var payload = new RegistroCompraBodegaDto
        {
            Bodega = 1,
            Fabricante = 1,
            Productos = []
        };

        var response = await client.PostAsJsonAsync("/compras", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        await app.StopAsync();
    }

    [Fact]
    public async Task RegistrarCompra_DebeRetornar401_SiNoHayToken()
    {
        SinAutenticacion();

        var payload = new RegistroCompraBodegaDto
        {
            Bodega = 1,
            Fabricante = 1,
            Productos = new List<ProductoAComprarDto> { new(1, 2) }
        };

        var response = await _client.PostAsJsonAsync("/compras", payload);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RegistrarCompra_DebeRetornar403_CuandoRolEsIncorrecto()
    {
        AutenticarComoCliente();

        var payload = new RegistroCompraBodegaDto
        {
            Bodega = 1,
            Fabricante = 1,
            Productos = new List<ProductoAComprarDto> { new(1, 2) }
        };

        var response = await _client.PostAsJsonAsync("/compras", payload);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task RegistrarCompra_DebeRetornar400_SiLaBodegaNoExiste()
    {
        var app = TestApplicationFactory.Create(Guid.NewGuid().ToString());
        await app.StartAsync();
        var client = app.GetTestClient();

        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var payload = new RegistroCompraBodegaDto
        {
            Bodega = 9999, // bodega inexistente
            Fabricante = 1,
            Productos = new List<ProductoAComprarDto> { new(1, 5) }
        };

        var response = await client.PostAsJsonAsync("/compras", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var contenido = await response.Content.ReadAsStringAsync();
        Assert.Contains("bodega", contenido, StringComparison.OrdinalIgnoreCase);

        await app.StopAsync();
    }

    [Fact]
    public async Task RegistrarCompra_DebeActualizarInventarioCorrectamente()
    {
        var app = TestApplicationFactory.Create(Guid.NewGuid().ToString());
        await app.StartAsync();
        var client = app.GetTestClient();

        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InventariosDbContext>();
        db.Bodegas.Add(new Bodega(1, "Bodega Test"));
        db.Inventarios.Add(new Inventario(1, 1, 10)); // stock inicial
        await db.SaveChangesAsync();

        var payload = new RegistroCompraBodegaDto
        {
            Bodega = 1,
            Fabricante = 1,
            Productos = new List<ProductoAComprarDto> { new(1, 5) }
        };

        var response = await client.PostAsJsonAsync("/compras", payload);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verificamos nuevo estado del inventario
        using var verifyScope = app.Services.CreateScope();
        var dbVerif = verifyScope.ServiceProvider.GetRequiredService<InventariosDbContext>();
        var inventario = await dbVerif.Inventarios.FindAsync(1, 1);

        Assert.NotNull(inventario);
        Assert.Equal(15, inventario!.Cantidad);

        await app.StopAsync();
    }


    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }

    private void AutenticarComoCcp()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }

    private void AutenticarComoCliente()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenCliente();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }

    private void SinAutenticacion()
    {
        _client.DefaultRequestHeaders.Authorization = null;
    }
}

internal record BodegaResponse(int id, string descripcion);

internal record CompraRegistradaResponse(string mensaje, int totalProductos);