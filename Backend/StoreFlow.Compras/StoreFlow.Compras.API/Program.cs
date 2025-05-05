using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Infraestructura;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.Endpoints;
using StoreFlow.Compras.API.Servicios;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


var connectionString = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("CONNECTION_STRING_COMPRAS");


builder.Services.ConfigurarAutenticacion();
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("SoloUsuariosCcp", policy =>
        policy.RequireRole("UsuarioCcp"));
    
    opciones.AddPolicy("Vendedor" , policy =>
        policy.RequireRole("Vendedor"));
    
    opciones.AddPolicy("UsuariosCcpOVendedor", policy =>
            policy.RequireRole("UsuarioCcp", "Vendedor"));
});

builder.Services.ConfigurarMasstransitRabbitMq(Assembly.GetExecutingAssembly());
builder.Host.ConfigurarObservabilidad("Compras");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ComprasDbContext>(options => { options.UseNpgsql(connectionString); });


builder.Services.AddScoped<IFabricantesService, FabricantesService>();
builder.Services.AddScoped<IProductosService, ProductosService>();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapComprasEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ComprasDbContext>();
    db.Database.Migrate();
}

app.Run();


[ExcludeFromCodeCoverage]
public partial class Program
{
}