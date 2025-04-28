using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Entidades;
using StoreFlow.Compras.Tests.Utilidades;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StoreFlow.Compras.Tests.Endpoints;

public class ProductosEndpointTests : IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebApplication _app = null!;

    public async Task InitializeAsync()
    {
        _app = TestApplicationFactory.Create(Guid.NewGuid().ToString());
        await _app.StartAsync();
        _client = _app.GetTestClient();


        using var scope = _app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ComprasDbContext>();
        db.Fabricantes.Add(new Fabricante
        {
            Id = 1,
            RazonSocial = "Fabricante 1",
            CorreoElectronico = "fabricante1@empresa.com"
        });
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task DebeRetornarCreated_CuandoLaSolicitudEsValida()
    {
        var request = new CrearProductoRequest("Producto A", 1, "SKU001", 10000, "https://imagen.com/a.jpg");
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.PostAsJsonAsync("/productos", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task DebeRetornarConflict_CuandoElSkuYaExiste()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var request = new CrearProductoRequest("Producto A", 1, "SKU_DUP", 10000, "https://imagen.com/a.jpg");
        await _client.PostAsJsonAsync("/productos", request);

        var response = await _client.PostAsJsonAsync("/productos", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DebeRetornarValidationProblem_CuandoElNombreEsNulo()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var request = new
        {
            nombre = (string?)null,
            fabricanteAsociado = 1,
            codigo = "SKU002",
            precio = 10000,
            imagen = "https://imagen.com/b.jpg"
        };

        var response = await _client.PostAsJsonAsync("/productos", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("https://")]
    public async Task DebeRetornarValidationProblem_CuandoLaImagenEsInvalida(string? url)
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var request = new CrearProductoRequest("Producto Imagen", 1, "SKU003", 15000, url!);
        var response = await _client.PostAsJsonAsync("/productos", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DebeRetornarValidationProblem_CuandoElPrecioEsCero()
    {
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var request = new CrearProductoRequest("Producto Cero", 1, "SKU004", 0, "https://imagen.com/c.jpg");
        var response = await _client.PostAsJsonAsync("/productos", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DebeRetornarUnauthorized_CuandoNoHayToken()
    {
        var request = new CrearProductoRequest("Producto", 1, "SKU005", 20000, "https://imagen.com/d.jpg");
        _client.DefaultRequestHeaders.Authorization = null;

        var response = await _client.PostAsJsonAsync("/productos", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DebeRetornarForbidden_CuandoElRolNoEsUsuarioCcp()
    {
        var request = new CrearProductoRequest("Producto", 1, "SKU006", 20000, "https://imagen.com/e.jpg");
        var jwt = GeneradorTokenPruebas.GenerarTokenVendedor();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.PostAsJsonAsync("/productos", request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Fact]
    public async Task DebeRetornarOk_CuandoSeObtienenProductos()
    {
    
        using var scope = _app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ComprasDbContext>();
        db.Productos.Add(new Producto
        {
            Id = 1,
            Nombre = "Producto 1",
            Sku = "SKU001",
            Precio = 10000,
            ImagenUrl = "https://imagen.com/a.jpg",
            FabricanteId = 1
        });
        
        db.Productos.Add(new Producto
        {
            Id = 2,
            Nombre = "Producto 2",
            Sku = "SKU002",
            Precio = 20000,
            ImagenUrl = "https://imagen.com/producto2.jpg",
            FabricanteId = 1
        });
        await db.SaveChangesAsync();
    
        var response = await _client.GetAsync("/productos");
    
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var productos = await response.Content.ReadFromJsonAsync<ProductoResponse[]>();
        Assert.NotNull(productos);
        Assert.Equal(2, productos.Length);
        Assert.Equal("Producto 1", productos[0].Nombre);
        Assert.Equal("1", productos[0].Codigo);
        Assert.Equal("Producto 2", productos[1].Nombre);
        Assert.Equal(10000, productos[0].Precio);
        Assert.Equal("https://imagen.com/a.jpg", productos[0].Imagen);
        
    }

    public async Task DisposeAsync()
    {
        using var scope = _app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ComprasDbContext>();

        db.Fabricantes.RemoveRange(db.Fabricantes);
        await db.SaveChangesAsync();

        await _app.StopAsync();
    }
}