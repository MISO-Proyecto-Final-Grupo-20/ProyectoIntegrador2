using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.Endpoints;
using StoreFlow.Compras.API.Servicios;
using StoreFlow.Compras.Tests.Utilidades;
using System.Security.Claims;
using System.Text;

namespace StoreFlow.Compras.Tests
{
    public static class TestApplicationFactory
    {
        public static WebApplication Create(string? databaseName = null)
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
            });

            var nombreDb = databaseName ?? "TestDb";

            builder.WebHost.UseTestServer();

            builder.Services.AddDbContext<ComprasDbContext>(options =>
                options.UseInMemoryDatabase(nombreDb));

            builder.Services.AddScoped<IFabricantesService, FabricantesService>();
            builder.Services.AddScoped<IProductosService, ProductosService>();


            var app = builder.Build();

            app.MapComprasEndpoints();

            return app;
        }
    }
}