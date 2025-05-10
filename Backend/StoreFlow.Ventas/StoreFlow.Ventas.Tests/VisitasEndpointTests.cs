using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StoreFlow.Ventas.Tests;

public class VisitasEndpointTests : IAsyncLifetime
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
    public async Task RegistrarVisita_DebeRetornarOk_CuandoSolicitudEsValida()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenVendedor();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var contenidoBytes = new byte[] { 0x00, 0x01, 0x02 };
        var stream = new MemoryStream(contenidoBytes);
        var videoContent = new StreamContent(stream);
        videoContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");

        using var form = new MultipartFormDataContent();
        form.Add(videoContent, "video", "visita.mp4");
        form.Add(new StringContent("1"), "idVendedor");
        form.Add(new StringContent("1"), "idCliente");

        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas")
        {
            Content = form
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<VisitaRegistradaResponse>();
        Assert.NotNull(body);
        Assert.Equal("Visita registrada con éxito.", body!.Mensaje);
        Assert.True(body.Id > 0);
        Assert.NotEmpty(body.UrlVideo);
        Assert.Equal("Pendiente", body.Estado);
    }

    [Fact]
    public async Task RegistrarVisita_DebeRetornar400_CuandoNoHayArchivo()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenVendedor();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent("1"), "idVendedor");
        form.Add(new StringContent("1"), "idCliente");

        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas")
        {
            Content = form
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("video", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RegistrarVisita_DebeRetornar400_CuandoNoHayIdVendedor()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenVendedor();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var contenidoBytes = new byte[] { 0x01 };
        var videoContent = new StreamContent(new MemoryStream(contenidoBytes));
        videoContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");

        using var form = new MultipartFormDataContent();
        form.Add(videoContent, "video", "visita.mp4");
        form.Add(new StringContent("1"), "idCliente");

        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas")
        {
            Content = form
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("idVendedor", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RegistrarVisita_DebeRetornar400_CuandoNoHayIdCliente()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenVendedor();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var contenidoBytes = new byte[] { 0x01 };
        var videoContent = new StreamContent(new MemoryStream(contenidoBytes));
        videoContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");

        using var form = new MultipartFormDataContent();
        form.Add(videoContent, "video", "visita.mp4");
        form.Add(new StringContent("1"), "idVendedor");

        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas")
        {
            Content = form
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("idCliente", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RegistrarVisita_DebeRetornar401_SiNoHayToken()
    {
        var contenidoBytes = new byte[] { 0x01 };
        var videoContent = new StreamContent(new MemoryStream(contenidoBytes));
        videoContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");

        using var form = new MultipartFormDataContent();
        form.Add(videoContent, "video", "visita.mp4");
        form.Add(new StringContent("1"), "idVendedor");
        form.Add(new StringContent("1"), "idCliente");

        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas")
        {
            Content = form
        };


        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RegistrarVisita_DebeRetornar403_CuandoRolNoEsVendedor()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenCliente();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var contenidoBytes = new byte[] { 0x01 };
        var videoContent = new StreamContent(new MemoryStream(contenidoBytes));
        videoContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");

        using var form = new MultipartFormDataContent();
        form.Add(videoContent, "video", "visita.mp4");
        form.Add(new StringContent("1"), "idVendedor");
        form.Add(new StringContent("1"), "idCliente");

        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas")
        {
            Content = form
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }


    public record VisitaRegistradaResponse(int Id, DateTime Fecha, string UrlVideo, string Estado, string Mensaje);


    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}