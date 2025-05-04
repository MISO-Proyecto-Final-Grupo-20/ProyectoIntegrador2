using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using StoreFlow.Ventas.API.EndPoints;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MassTransit;
using NSubstitute;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.DTOs;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.Tests;

public class CrearPedidosEndPointTests : IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebApplication _app = null!;
    private IPublishEndpoint? _publishEndpointMock;
    private VentasDbContext _dbContext = null!;

    public async Task InitializeAsync()
    {
        _publishEndpointMock = Substitute.For<IPublishEndpoint>();
        _app = TestApplicationFactory.Create(_publishEndpointMock, new DateTime(2025, 4, 27));
        await _app.StartAsync();
        _client = _app.GetTestClient();

        _dbContext = (VentasDbContext) _app.Services.GetService(typeof(VentasDbContext))!;
    }

    [Fact]
    public async Task CrearPedido_RetornaAccepted()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenCliente();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.PostAsJsonAsync<ProductoPedidoRequest[]>("/pedidos",
        [
            new ProductoPedidoRequest("1", 2, 10),
            new ProductoPedidoRequest("2", 3, 20)
        ]);

        await _publishEndpointMock!.Received(1).Publish(Arg.Is<ProcesarPedido>(p => p.solicitud.IdCliente == 3));

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    [Fact]
    public async Task CrearPedido_SinToken_RetornaUnauthorazed()
    {
        var response = await _client.PostAsJsonAsync<ProductoPedidoRequest[]>("/pedidos",
        [
            new ProductoPedidoRequest("1", 2, 10),
            new ProductoPedidoRequest("2", 3, 20)
        ]);


        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ObtenerPedidosDeCliente_RetornaOk()
    {
        await _dbContext.GuardarPedidoAsync(
            new Pedido(3,
                new DateTime(2025, 4, 27),
                [
                    new ProductoPedido(1, 2, 10_000, false, null, null, null),
                    new ProductoPedido(2, 3, 20_000, true, null, null, null)
                ], "nombre cliente 3", "direccion cliente 3", null, null));
        
        await _dbContext.GuardarPedidoAsync(
            new Pedido(2,
                new DateTime(2025, 5, 3),
                [
                    new ProductoPedido(1, 5, 10_000, true, null, null, null),
                    new ProductoPedido(2, 20, 20_000, true, null, null, null)
                ],"nombreCliente 2", "direccion cliente 2", null, null));
        
        var jwt = GeneradorTokenPruebas.GenerarTokenCliente();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await _client.GetAsync("/pedidos/pendientes");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        
        var pedidos = await response.Content.ReadFromJsonAsync<List<PedidoResponse>>();
        
        
        Assert.NotNull(pedidos);
        Assert.Single(pedidos);
        Assert.Equal(3, pedidos[0].IdCliente);
        Assert.Equal("nombre cliente 3", pedidos[0].NombreCliente);
        Assert.Equal("direccion cliente 3", pedidos[0].LugarEntrega);
        Assert.Single(pedidos[0].Productos);
        Assert.Equal(2, pedidos[0].Productos[0].Id);
      
    }

    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}