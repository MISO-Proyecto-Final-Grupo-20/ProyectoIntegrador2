using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Infraestructura;
using StoreFlow.Inventarios.API.Datos;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("CONNECTION_STRING_INVENTARIOS");

builder.Services.ConfigurarAutenticacion();
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("SoloUsuariosCcp", policy =>
        policy.RequireRole("UsuarioCcp"));
});

builder.Services.ConfigurarMasstransitRabbitMq(Assembly.GetExecutingAssembly());
builder.Host.ConfigurarObservabilidad("Inventarios");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<InventariosDbContext>(options => { options.UseNpgsql(connectionString); });


var app = builder.Build();


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InventariosDbContext>();
    db.Database.Migrate();
}

app.Run();


[ExcludeFromCodeCoverage]
public partial class Program
{
}