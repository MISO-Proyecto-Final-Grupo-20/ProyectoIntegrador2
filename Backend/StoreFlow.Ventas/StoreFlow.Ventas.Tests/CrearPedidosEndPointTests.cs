using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

    [Fact]
    public async Task CrearPlanesDeCuentas_RetornaOk()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.PostAsJsonAsync("/planesVenta",
        new CrearPlanVentaRequest(1, 15_000_000, [
            new(1, "Vendedor 1"), 
            new(2, "Vendedor 2")
        ]));
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var planDeVentasCreado = _dbContext.PlanesDeVentas.OrderBy(p => p.IdVendedor).ToList();
        
        Assert.Equal(2, planDeVentasCreado.Count);
        Assert.Equal(1, planDeVentasCreado[0].IdVendedor);
        Assert.Equal("Vendedor 1", planDeVentasCreado[0].NombreVendedor);
        Assert.Equal(Periodicidad.Mensual, planDeVentasCreado[0].PeriodoTiempo);
        Assert.Equal(15_000_000, planDeVentasCreado[0].MetaVentas);
    }
    
    [Fact]
    public async Task CrearPlanesDeCuentas_EnPeriodoYVendedor_AcutalizaMeta_RetornaOk()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        _ = await _client.PostAsJsonAsync("/planesVenta",
            new CrearPlanVentaRequest(1, 15_000_000, [
                new(1, "Vendedor 1"), 
                new(2, "Vendedor 2")
            ]));
        
        var response = await _client.PostAsJsonAsync("/planesVenta",
            new CrearPlanVentaRequest(1, 30_000_000, [
                new(1, "Vendedor 1"), 
            ]));
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var planDeVentasCreado = _dbContext.PlanesDeVentas.OrderBy(p => p.IdVendedor).ToList();
        
        Assert.Equal(2, planDeVentasCreado.Count);
        Assert.Equal(1, planDeVentasCreado[0].IdVendedor);
        Assert.Equal("Vendedor 1", planDeVentasCreado[0].NombreVendedor);
        Assert.Equal(Periodicidad.Mensual, planDeVentasCreado[0].PeriodoTiempo);
        Assert.Equal(30_000_000, planDeVentasCreado[0].MetaVentas);
        Assert.Equal(15_000_000, planDeVentasCreado[1].MetaVentas);
    }

    [Fact]
    public async Task ObtenerPeriodosDeTiempo_RetornaOk()
    {
        var response = await _client.GetAsync($"/periodosTiempo");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var periodos = await response.Content.ReadFromJsonAsync<List<PeriodoTiempoResponse>>();
        Assert.NotNull(periodos);
        Assert.Equal(4, periodos.Count);
        Assert.Equal((int)Periodicidad.Mensual, periodos[0].Id);
        Assert.Equal("Mensual", periodos[0].Descripcion);
        Assert.Equal((int)Periodicidad.Trimestral, periodos[1].Id);
        Assert.Equal("Trimestral", periodos[1].Descripcion);
        Assert.Equal((int)Periodicidad.Semestral, periodos[2].Id);
        Assert.Equal("Semestral", periodos[2].Descripcion);
        Assert.Equal((int)Periodicidad.Anual, periodos[3].Id);
        Assert.Equal("Anual", periodos[3].Descripcion);
    }

    [Fact]
    public async Task ObtenerReporteVentas_RetornaOk()
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
                    new ProductoPedido(1, 5, 10_000, true,"codigo 1", "producto 1", "imagen 1"),
                    new ProductoPedido(2, 20, 20_000, true, "codigo 2", "producto 2", "imangen 2")
                ],"nombreCliente 2", "direccion cliente 2", 1, "Vendedor 1"));
        
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await _client.PostAsJsonAsync("/consultaInformes", new ReporteVentasRequest(1, new DateTime(2025, 5, 3), new DateTime(2025, 5, 3), 1));
        var reporte = await response.Content.ReadFromJsonAsync<ReporteVentasResponse[]>();
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(reporte);
        Assert.Single(reporte);
        Assert.Equivalent(new ReporteVentasResponse("Vendedor 1", new DateTime(2025, 5, 3), "producto 1", 5), reporte.First());
        
    }

    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}