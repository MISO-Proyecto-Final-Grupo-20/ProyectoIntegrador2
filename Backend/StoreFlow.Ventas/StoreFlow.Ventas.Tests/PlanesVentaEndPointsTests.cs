using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.DTOs;
using StoreFlow.Ventas.API.Entidades;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace StoreFlow.Ventas.Tests;

public class PlanesVentaEndPointsTests : IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebApplication _app = null!;
    private IPublishEndpoint? _publishEndpointMock;
    private VentasDbContext _dbContext = null!;

    public async Task InitializeAsync()
    {
        _publishEndpointMock = Substitute.For<IPublishEndpoint>();
        _app = TestApplicationFactory.Create(_publishEndpointMock, new DateTime(2025, 4, 27));
        
        // Seed the database with test data
        using (var scope = _app.Services.CreateScope())
        {
            _dbContext = scope.ServiceProvider.GetRequiredService<VentasDbContext>();
            
            _dbContext.PeriodosTiempo.Add(new PeriodoTiempo("Mensual"));
            _dbContext.PeriodosTiempo.Add(new PeriodoTiempo("Trimestral"));
            _dbContext.PeriodosTiempo.Add(new PeriodoTiempo("Semestral"));
            
            _dbContext.PlanesVenta.Add(new PlanVenta("Plan Test", "Plan de prueba", 100.0m, 1));
            
            await _dbContext.SaveChangesAsync();
        }
        
        await _app.StartAsync();
        _client = _app.GetTestClient();
    }
    
    [Fact]
    public async Task ObtenerPlanesVenta_SinToken_RetornaUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/planesVenta");
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task ObtenerPlanesVenta_ConTokenCcp_RetornaPlanes()
    {
        // Arrange
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        // Act
        var response = await _client.GetAsync("/planesVenta");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var planes = await response.Content.ReadFromJsonAsync<List<PlanVentaDto>>();
        Assert.NotNull(planes);
        
        var plan = planes.FirstOrDefault(x => x.Id == 1);
        Assert.NotNull(plan);
    }
    
    [Fact]
    public async Task ObtenerPlanVentaPorId_IdInexistente_RetornaNotFound()
    {
        // Arrange
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        // Act
        var response = await _client.GetAsync("/planesVenta/999");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task CrearPlanVenta_ConTokenCcp_RetornaCreated()
    {
        // Arrange
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        var nuevoPlan = new CrearPlanVentaDto(
            "Nuevo Plan",
            "Descripción del nuevo plan",
            200.0m,
            2);
        
        // Act
        var response = await _client.PostAsJsonAsync("/planesVenta", nuevoPlan);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var planCreado = await response.Content.ReadFromJsonAsync<PlanVentaDto>();
        Assert.NotNull(planCreado);
        Assert.Equal("Nuevo Plan", planCreado.Nombre);
        Assert.Equal("Descripción del nuevo plan", planCreado.Descripcion);
        Assert.Equal(200.0m, planCreado.Precio);
        Assert.Equal(2, planCreado.PeriodoTiempoId);
        Assert.Equal("Trimestral", planCreado.NombrePeriodo);
        
        // Verify it's in the database
        using (var scope = _app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<VentasDbContext>();
            var planEnDb = await dbContext.PlanesVenta
                .Include(p => p.PeriodoTiempo)
                .FirstOrDefaultAsync(p => p.Nombre == "Nuevo Plan");
                
            Assert.NotNull(planEnDb);
            Assert.Equal("Descripción del nuevo plan", planEnDb.Descripcion);
            Assert.Equal(200.0m, planEnDb.Precio);
            Assert.Equal(2, planEnDb.PeriodoTiempoId);
            Assert.Equal("Trimestral", planEnDb.PeriodoTiempo.Nombre);
        }
    }
    
    [Fact]
    public async Task CrearPlanVenta_PeriodoInexistente_RetornaBadRequest()
    {
        // Arrange
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        var nuevoPlan = new CrearPlanVentaDto(
            "Plan Inválido",
            "Este plan no se debe crear",
            300.0m,
            999); // ID inexistente
        
        // Act
        var response = await _client.PostAsJsonAsync("/planesVenta", nuevoPlan);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task ActualizarPlanVenta_ConTokenCcp_RetornaOk()
    {
        // Arrange
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        var planActualizado = new ActualizarPlanVentaDto(
            "Plan Actualizado",
            "Descripción actualizada",
            150.0m,
            3);
        
        // Act
        var response = await _client.PutAsJsonAsync("/planesVenta/1", planActualizado);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var plan = await response.Content.ReadFromJsonAsync<PlanVentaDto>();
        Assert.NotNull(plan);
        Assert.Equal(1, plan.Id);
        Assert.Equal("Plan Actualizado", plan.Nombre);
        Assert.Equal("Descripción actualizada", plan.Descripcion);
        Assert.Equal(150.0m, plan.Precio);
        Assert.Equal(3, plan.PeriodoTiempoId);
        Assert.Equal("Semestral", plan.NombrePeriodo);
        
        // Verify it's updated in the database
        using (var scope = _app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<VentasDbContext>();
            var planEnDb = await dbContext.PlanesVenta
                .Include(p => p.PeriodoTiempo)
                .FirstOrDefaultAsync(p => p.Id == 1);
                
            Assert.NotNull(planEnDb);
            Assert.Equal("Plan Actualizado", planEnDb.Nombre);
            Assert.Equal("Descripción actualizada", planEnDb.Descripcion);
            Assert.Equal(150.0m, planEnDb.Precio);
            Assert.Equal(3, planEnDb.PeriodoTiempoId);
            Assert.Equal("Semestral", planEnDb.PeriodoTiempo.Nombre);
        }
    }
    
    [Fact]
    public async Task EliminarPlanVenta_ConTokenCcp_RetornaNoContent()
    {
        // Arrange
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        // Act
        var response = await _client.DeleteAsync("/planesVenta/1");
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
        // Verify it's deleted from the database
        using (var scope = _app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<VentasDbContext>();
            var planExiste = await dbContext.PlanesVenta.AnyAsync(p => p.Id == 1);
            
            Assert.False(planExiste);
        }
    }
    
    [Fact]
    public async Task EliminarPlanVenta_IdInexistente_RetornaNotFound()
    {
        // Arrange
        var jwt = GeneradorTokenPruebas.GenerarTokenUsuarioCcp();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        // Act
        var response = await _client.DeleteAsync("/planesVenta/999");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    public async Task DisposeAsync()
    {
        await _app.StopAsync();
    }
}