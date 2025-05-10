using StoreFlow.Compartidos.Core.Infraestructura;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Logistica.API.Datos;

var builder = WebApplication.CreateBuilder(args);

var connectionString = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("CONNECTION_STRING_LOGISTICA");

builder.Services.ConfigurarAutenticacion();
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("SoloUsuariosCcp", policy =>
        policy.RequireRole("UsuarioCcp"));
    
    opciones.AddPolicy("Cliente", policy =>
        policy.RequireRole("Cliente"));

    opciones.AddPolicy("Vendedor", policy =>
        policy.RequireRole("Vendedor"));
});
builder.Services.AddDbContext<LogisticaDbContext>(options => { options.UseNpgsql(connectionString); });
builder.Services.ConfigurarMasstransitRabbitMq(Assembly.GetExecutingAssembly());
builder.Host.ConfigurarObservabilidad("Logistica");


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LogisticaDbContext>();
    db.Database.Migrate();
}

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}