using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.EndPoints;
using StoreFlow.Ventas.API.Servicios;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using StoreFlow.Compartidos.Core.Infraestructura;

namespace StoreFlow.Ventas.Tests;

public static class TestApplicationFactory
{
    public static WebApplication Create(IPublishEndpoint publishEndpoint, DateTime fecha,
        string dbName = "VentasTestDb")
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ApplicationName = typeof(Program).Assembly.FullName,
            EnvironmentName = "Testing"
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = ClaimTypes.Role,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GeneradorTokenPruebas.ClavePruebas))
                };
            });

        builder.Services.AddAuthorization(opciones =>
        {
            opciones.AddPolicy("SoloUsuariosCcp", policy =>
                policy.RequireRole("UsuarioCcp"));
            opciones.AddPolicy("Cliente", policy =>
                policy.RequireRole("Cliente"));
            opciones.AddPolicy("Vendedor", policy =>
                policy.RequireRole("Vendedor"));
        });


        builder.Services.AddSingleton(publishEndpoint);

        builder.WebHost.UseTestServer();

        builder.Services.AddDbContext<VentasDbContext>(options =>
            options.UseInMemoryDatabase(dbName));

        builder.Services.AddScoped<IDateTimeProvider>(_ => new TestDateTimeProvider(fecha));
        builder.Services.AddScoped<IBlobStorageService, TestBlobStorageService>();


        var app = builder.Build();


        app.MapCrearPedidoEndPont();
        app.MapPlanesDeVentasEndPoints();
        app.MapVisitasEndPoints();


        return app;
    }

    public class TestDateTimeProvider(DateTime utcNow) : IDateTimeProvider
    {
        public DateTime UtcNow { get; } = utcNow;
    }

    public class TestBlobStorageService : IBlobStorageService
    {
        public Task<string> SubirVideoAsync(IFormFile archivo, string nombreArchivo)
        {
            var urlSimulada = $"https://fake.blob.core.windows.net/visitas/{nombreArchivo}";
            return Task.FromResult(urlSimulada);
        }
    }
}