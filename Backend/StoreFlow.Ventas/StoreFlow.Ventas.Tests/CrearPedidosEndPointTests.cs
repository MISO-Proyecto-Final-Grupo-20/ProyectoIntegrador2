using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using StoreFlow.Ventas.API.EndPoints;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StoreFlow.Ventas.Tests;

public class CrearPedidosEndPointTests : IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebApplication _app = null!;

    public async Task InitializeAsync()
    {
        _app = TestApplicationFactory.Create(
        );
        await _app.StartAsync();
        _client = _app.GetTestClient();
        
        
    }


    [Fact]
    public async Task CrearPedido_RetornaAccepted()
    {
        
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        var response = await _client.PostAsJsonAsync("/pedido", new CrearPedidoRequest(1, new []
        {
            new ProductoPedidoRequest("1", 2, 10),
            new ProductoPedidoRequest("2", 3, 20)
        }));

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}