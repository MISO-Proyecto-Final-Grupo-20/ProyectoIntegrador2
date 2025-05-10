using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;

namespace StoreFlow.Ventas.Tests;

public class PlanesDeVentaEndPointsTests : IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebApplication _app = null!;
    private IPublishEndpoint? _publishEndpointMock;

    public async Task InitializeAsync()
    {
        _publishEndpointMock = Substitute.For<IPublishEndpoint>();
        _app = TestApplicationFactory.Create(_publishEndpointMock, new DateTime(2025, 4, 27),
            "PlanesDeVentaEndPointsTestsDb");
        await _app.StartAsync();
        _client = _app.GetTestClient();
    }

    [Fact]
    public async Task CrearPlanesVenta_DebeRetornarOk_CuandoSolicitudEsValida()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var request = new
        {
            PeriodoTiempo = 0,
            ValorVentas = 10000m,
            Vendedores = new[]
            {
                new { Id = 100, Nombre = "Juan Pérez" },
                new { Id = 200, Nombre = "Ana Gómez" }
            }
        };

        var response = await _client.PostAsJsonAsync("/planesVenta", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CrearPlanesVenta_DebeRetornarOk_CuandoNoHayVendedores()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var request = new
        {
            PeriodoTiempo = 0,
            ValorVentas = 10000m,
            Vendedores = Array.Empty<object>()
        };

        var response = await _client.PostAsJsonAsync("/planesVenta", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CrearPlanesVenta_DebeRetornar401_SiNoHayToken()
    {
        var request = new
        {
            PeriodoTiempo = 0,
            ValorVentas = 10000m,
            Vendedores = new[]
            {
                new { Id = 1, Nombre = "Juan" }
            }
        };

        var response = await _client.PostAsJsonAsync("/planesVenta", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CrearPlanesVenta_DebeRetornar403_CuandoRolEsInvalido()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenCliente();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var request = new
        {
            PeriodoTiempo = 0,
            ValorVentas = 10000m,
            Vendedores = new[]
            {
                new { Id = 1, Nombre = "Juan" }
            }
        };

        var response = await _client.PostAsJsonAsync("/planesVenta", request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }


    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}