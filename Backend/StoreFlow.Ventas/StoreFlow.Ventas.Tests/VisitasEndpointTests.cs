using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.DTOs;
using StoreFlow.Ventas.API.Entidades;

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
        form.Add(new StringContent("Sun May 11 2025 00:00:00 GMT-0500 (hora estándar de Colombia)"), "fecha");
        form.Add(new StringContent("01:01"), "hora");

        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas/1")
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
        form.Add(new StringContent("2025-05-01"), "fecha");
        form.Add(new StringContent("01:01"), "hora");


        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas/1")
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
    public async Task RegistrarVisita_DebeRetornar401_SiNoHayToken()
    {
        var contenidoBytes = new byte[] { 0x01 };
        var videoContent = new StreamContent(new MemoryStream(contenidoBytes));
        videoContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");

        using var form = new MultipartFormDataContent();
        form.Add(videoContent, "video", "visita.mp4");
        form.Add(new StringContent("2025-05-01"), "fecha");
        form.Add(new StringContent("01:01"), "hora");

        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas/1")
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
        form.Add(new StringContent("2025-05-01"), "fecha");
        form.Add(new StringContent("01:01"), "hora");


        var request = new HttpRequestMessage(HttpMethod.Post, "/visitas/1")
        {
            Content = form
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }


    [Fact]
    public async Task ConsultarVisitas_DebeRetornarOk_ConVisitasRegistradas()
    {
        using var scope = _app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VentasDbContext>();

        db.Visitas.Add(new Visita
        {
            IdVendedor = 2,
            IdCliente = 2,
            Fecha = DateTime.UtcNow,
            Video = new Video
            {
                Url = "https://fake.blob/visita1.mp4",
                Estado = EstadoProcesamiento.Procesado,
                Recomendacion = "Mover productos al centro"
            }
        });
        await db.SaveChangesAsync();

        var jwt = GeneradorTokenPruebas.GenerarTokenVendedor();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.GetAsync("/visitas/2");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var visitas = await response.Content.ReadFromJsonAsync<List<VisitaResponse>>();
        Assert.NotNull(visitas);
        Assert.Single(visitas);
    }

    [Fact]
    public async Task ConsultarVisitas_DebeRetornarOk_SinVisitas()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenVendedor();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.GetAsync("/visitas/88");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var visitas = await response.Content.ReadFromJsonAsync<List<VisitaResponse>>();
        Assert.NotNull(visitas);
        Assert.Empty(visitas);
    }

    [Fact]
    public async Task ConsultarVisitas_DebeRetornar401_SiNoHayToken()
    {
        var response = await _client.GetAsync("/visitas/2");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ConsultarVisitas_DebeRetornar403_CuandoRolEsInvalido()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenCliente();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.GetAsync("/visitas/2");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ConsultarVisitasAnalisis_DebeRetornarOk_ConVisitasProcesadas()
    {
        var app = TestApplicationFactory.Create(_publishEndpointMock, new DateTime(2025, 4, 27),
            Guid.NewGuid().ToString());
        await app.StartAsync();
        var client = app.GetTestClient();
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VentasDbContext>();

        db.Visitas.Add(new Visita
        {
            IdVendedor = 2,
            IdCliente = 2,
            Fecha = new DateTime(2025, 5, 11, 11, 11, 0, DateTimeKind.Utc),
            Video = new Video
            {
                Url = "https://fake.blob/visita1.mp4",
                Estado = EstadoProcesamiento.Procesado,
                NombreOriginal = "Estantesvacios.mp4",
                TamanioBytes = 12345678,
                Recomendacion = "Rotar productos cada semana"
            }
        });
        await db.SaveChangesAsync();

        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await client.GetAsync("/visitas/analisis");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var visitas = await response.Content.ReadFromJsonAsync<List<AnalisisVisitaResponse>>();
        Assert.NotNull(visitas);
        Assert.Single(visitas);
        Assert.Equal("Estantesvacios.mp4", visitas[0].Archivo.Nombre);
        Assert.Equal(12345678, visitas[0].Archivo.Tamanio);
        Assert.Equal("Rotar productos cada semana", visitas[0].Observaciones);

        await app.StopAsync();
    }

    [Fact]
    public async Task ConsultarVisitasAnalisis_DebeRetornarOk_SinVisitas()
    {
        var app = TestApplicationFactory.Create(_publishEndpointMock, new DateTime(2025, 4, 27),
            Guid.NewGuid().ToString());
        await app.StartAsync();
        var client = app.GetTestClient();
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VentasDbContext>();

        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await client.GetAsync("/visitas/analisis");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var visitas = await response.Content.ReadFromJsonAsync<List<AnalisisVisitaResponse>>();
        Assert.NotNull(visitas);
        Assert.Empty(visitas);
        await app.StopAsync();
    }

    [Fact]
    public async Task ActualizarObservacion_DebeRetornarOk_CuandoObservacionEsValida()
    {
        using var scope = _app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VentasDbContext>();

        var visita = new Visita
        {
            IdCliente = 1,
            IdVendedor = 1,
            Fecha = DateTime.UtcNow,
            Video = new Video
            {
                Url = "https://fake.blob/visita.mp4",
                Estado = EstadoProcesamiento.Procesado,
                NombreOriginal = "video.mp4",
                TamanioBytes = 1024,
                Recomendacion = "Inicial"
            }
        };
        db.Visitas.Add(visita);
        await db.SaveChangesAsync();

        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var contenido = new StringContent("Nueva recomendación actualizada", Encoding.UTF8, "text/plain");
        var response = await _client.PostAsync($"/visitas/analisis/{visita.Id}/observaciones", contenido);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("actualizada", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ActualizarObservacion_DebeRetornar404_SiVisitaNoExiste()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var contenido = new StringContent("Texto", Encoding.UTF8, "text/plain");
        var response = await _client.PostAsync("/visitas/analisis/9999/observaciones", contenido);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ActualizarObservacion_DebeRetornar400_SiTextoEsVacio()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var contenido = new StringContent("   ", Encoding.UTF8, "text/plain");
        var response = await _client.PostAsync("/visitas/analisis/1/observaciones", contenido);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ActualizarObservacion_DebeRetornar403_SiRolNoEsUsuarioCcp()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenVendedor();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var contenido = new StringContent("Intento no autorizado", Encoding.UTF8, "text/plain");
        var response = await _client.PostAsync("/visitas/analisis/1/observaciones", contenido);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ActualizarObservacion_DebeRetornar401_SiNoHayToken()
    {
        var contenido = new StringContent("Sin token", Encoding.UTF8, "text/plain");
        var response = await _client.PostAsync("/visitas/analisis/1/observaciones", contenido);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    public record VisitaRegistradaResponse(int Id, DateTime Fecha, string UrlVideo, string Estado, string Mensaje);


    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}