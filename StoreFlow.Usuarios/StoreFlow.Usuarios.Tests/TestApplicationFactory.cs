using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.Endpoints;
using StoreFlow.Usuarios.API.Infraestructura;

namespace StoreFlow.Usuarios.Tests
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

            builder.Services.AddDbContext<UsuariosDbContext>(options =>
                options.UseInMemoryDatabase("UsuariosTestDb"));


            builder.Services.AddSingleton<ProveedorToken>();

            var app = builder.Build();

            app.MapUsuariosEndpoints(); // Aquí se registra solo lo necesario

            return app;
        }
    }
}
