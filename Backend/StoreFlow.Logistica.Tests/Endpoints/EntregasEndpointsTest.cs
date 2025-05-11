using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Logistica.API.DTOs;
using StoreFlow.Logistica.API.Servicios;
using StoreFlow.Logistica.Tests.Utilidades;

namespace StoreFlow.Logistica.Tests.Endpoints;

public class EntregasEndpointsTest : IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebApplication _app = null!;
    private IEntregaServicio? _entregaServicioMock;

    public async Task InitializeAsync()
    {
        _entregaServicioMock = Substitute.For<IEntregaServicio>();
        _app = TestApplicationFactory.Create(_entregaServicioMock, Guid.NewGuid().ToString());
        await _app.StartAsync();
        _client = _app.GetTestClient();
    }

    [Fact]
    public async Task GetEntregasProgramadasEndPoint_RetornaOk()
    {
        EntregaProgramadaResponse[] entregas =
        [
            new(1, 191, new DateTime(2025, 5, 3), "Direccion cliente 1", 
                [
                    new ProductoPedidoResponse(1, 100, 1_000, "P001", "Producto 1", "url1"),
                ])
        ];
        
        _entregaServicioMock!.ObtenerEntregasClienteAsync(1).Returns(entregas);
        
        var jwt = GeneradorTokenPruebas.GenerarTokenCliente(); 
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await _client.GetAsync("/entregasProgramadas");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var entregasResponse = await response.Content.ReadFromJsonAsync<EntregaProgramadaResponse[]>();
        Assert.NotNull(entregasResponse);
        Assert.Equal(entregas.Length, entregasResponse!.Length);
        Assert.Equivalent(entregas[0], entregasResponse[0]);
            

    }

    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}