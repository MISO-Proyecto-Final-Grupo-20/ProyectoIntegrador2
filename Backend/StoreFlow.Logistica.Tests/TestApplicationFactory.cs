using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StoreFlow.Logistica.API.Datos;
using StoreFlow.Logistica.API.Endpoints;
using StoreFlow.Logistica.API.Servicios;
using StoreFlow.Logistica.Tests.Utilidades;

namespace StoreFlow.Logistica.Tests
{
    public static class TestApplicationFactory
    {
        public static WebApplication Create(IEntregaServicio entregaServicioMock, string? databaseName = null)
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
                
                opciones.AddPolicy("Vendedor" , policy =>
                    policy.RequireRole("Vendedor"));
                
                opciones.AddPolicy("Cliente", policy =>
                    policy.RequireRole("Cliente"));
            });

            var nombreDb = databaseName ?? "TestDb";

            builder.WebHost.UseTestServer();

            builder.Services.AddDbContext<LogisticaDbContext>(options =>
                options.UseInMemoryDatabase(nombreDb));

            builder.Services.AddScoped<IEntregaServicio>(_ => entregaServicioMock);


            var app = builder.Build();

            app.MapEntregasEndpoints();

            return app;
        }
    }
}