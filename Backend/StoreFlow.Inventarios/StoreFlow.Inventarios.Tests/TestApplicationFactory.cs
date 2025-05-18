using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StoreFlow.Inventarios.API.Datos;
using StoreFlow.Inventarios.API.Endpoints;
using StoreFlow.Inventarios.API.Servicios;

namespace StoreFlow.Inventarios.Tests;

public static class TestApplicationFactory
{
    public static WebApplication Create(string nombreBaseDatos = "InventarioTestDb")
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
        });

        builder.WebHost.UseTestServer();

        builder.Services.AddDbContext<InventariosDbContext>(options =>
            options.UseInMemoryDatabase(nombreBaseDatos));
        builder.Services.AddScoped<IRegistrarCompraService, RegistrarCompraService>();


        var app = builder.Build();

        app.MapInventariosEndpoints();
        app.MapComprasEndPoints();

        return app;
    }
}