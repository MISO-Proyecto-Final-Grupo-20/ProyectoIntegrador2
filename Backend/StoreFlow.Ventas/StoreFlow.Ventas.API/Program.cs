using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Infraestructura;
using StoreFlow.Ventas.API.Datos;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("CONNECTION_STRING");

builder.Services.ConfigurarAutenticacion();
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("SoloUsuariosCcp", policy =>
        policy.RequireRole("UsuarioCcp"));
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.ConfigurarMasstransitRabbitMq(Assembly.GetExecutingAssembly());
builder.Host.ConfigurarObservabilidad("Ventas");


builder.Services.AddDbContext<VentasDbContext>(options => { options.UseNpgsql(connectionString); });


var app = builder.Build();


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VentasDbContext>();
    db.Database.Migrate();
}

app.Run();


[ExcludeFromCodeCoverage]
public partial class Program
{
}