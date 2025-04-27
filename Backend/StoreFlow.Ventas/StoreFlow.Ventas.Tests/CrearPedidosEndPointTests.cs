using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using StoreFlow.Ventas.API.EndPoints;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MassTransit;
using NSubstitute;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Ventas.Tests;

public class CrearPedidosEndPointTests : IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebApplication _app = null!;
    private IPublishEndpoint? _publishEndpointMock;

    public async Task InitializeAsync()
    {
        _publishEndpointMock = Substitute.For<IPublishEndpoint>();
        _app = TestApplicationFactory.Create(_publishEndpointMock, new DateTime(2025, 4, 27));
        await _app.StartAsync();
        _client = _app.GetTestClient();
    }
    
    [Fact]
    public async Task CrearPedido_RetornaAccepted()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenCliente();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        var response = await _client.PostAsJsonAsync("/pedido", new CrearPedidoRequest( 
        [
            new ProductoPedidoRequest("1", 2, 10),
            new ProductoPedidoRequest("2", 3, 20)
        ]));
        
        await _publishEndpointMock!.Received(1).Publish(Arg.Is<ProcesarPedido>(p => p.solicitud.IdCliente == 3));
        
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }
    
    [Fact]
    public async Task CrearPedido_SinToken_RetornaUnauthorazed()
    {
        var response = await _client.PostAsJsonAsync("/pedido", new CrearPedidoRequest( 
        [
            new ProductoPedidoRequest("1", 2, 10),
            new ProductoPedidoRequest("2", 3, 20)
        ]));
        
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}