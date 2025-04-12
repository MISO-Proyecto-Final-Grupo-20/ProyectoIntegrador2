using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.Endpoints;
using StoreFlow.Compras.API.Servicios;

namespace StoreFlow.Compras.Tests
{
    public static class TestApplicationFactory
    {
        public static WebApplication Create()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ApplicationName = typeof(Program).Assembly.FullName,
                EnvironmentName = "Testing"
            });

            builder.WebHost.UseTestServer();

            builder.Services.AddDbContext<ComprasDbContext>(options =>
                options.UseInMemoryDatabase("UsuariosTestDb"));

            builder.Services.AddScoped<IFabricantesService, FabricantesService>();

            Environment.SetEnvironmentVariable("JWT_SECRET", "EstaEsUnaClaveSuperSecretaDe32Caracteres!");


            var app = builder.Build();

            app.MapComprasEndpoints();

            return app;
        }
    }
}
