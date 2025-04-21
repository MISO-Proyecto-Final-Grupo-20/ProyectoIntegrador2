using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Builder;
using StoreFlow.Ventas.API.EndPoints;

namespace StoreFlow.Ventas.Tests;

public class CrearPedidosEndPointTests : IAsyncLifetime
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
    public async Task CrearPedidoEndPont()
    {
        var response = await _client.PostAsJsonAsync("/pedido", new CrearPedidoCommand()); 

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

    }
    
    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }


}