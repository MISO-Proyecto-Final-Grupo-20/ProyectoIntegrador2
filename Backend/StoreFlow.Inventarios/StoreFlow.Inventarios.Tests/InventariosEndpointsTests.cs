using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using StoreFlow.Inventarios.API.DTOs;

namespace StoreFlow.Inventarios.Tests;

public class InventariosEndpointsTests : IAsyncLifetime
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
    public async Task ExisteInventario_RetornaOk()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenCliente();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        var response = await _client.PostAsJsonAsync("/existeProducto", new ExisteProductoRequest( 
       "1", 10));
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var existeProducto = await response.Content.ReadFromJsonAsync<bool>();
        Assert.False(existeProducto);
    }
    
    [Fact]
    public async Task ExisteInventario_SinToken_RetornaUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/existeProducto", new ExisteProductoRequest( 
            "1", 10));
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}