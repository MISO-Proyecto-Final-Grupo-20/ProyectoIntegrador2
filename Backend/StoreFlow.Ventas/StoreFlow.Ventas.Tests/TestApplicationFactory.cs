using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.EndPoints;
using System.Security.Claims;
using System.Text;

namespace StoreFlow.Ventas.Tests;

public static class TestApplicationFactory
{
    public static WebApplication Create(IPublishEndpoint publishEndpoint)
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

        

        builder.Services.AddSingleton(publishEndpoint);

        builder.WebHost.UseTestServer();

        builder.Services.AddDbContext<VentasDbContext>(options =>
            options.UseInMemoryDatabase("VentasTestDb"));


        var app = builder.Build();
        

        app.MapCrearPedidoEndPont();


        return app;
    }
}