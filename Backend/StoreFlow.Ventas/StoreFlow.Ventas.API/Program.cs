using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Infraestructura;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.EndPoints;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using StoreFlow.Ventas.API.Servicios;

var builder = WebApplication.CreateBuilder(args);

var connectionString = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("CONNECTION_STRING_VENTAS");

builder.Services.ConfigurarAutenticacion();
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("SoloUsuariosCcp", policy =>
        policy.RequireRole("UsuarioCcp"));
    
    opciones.AddPolicy("Cliente" , policy =>
        policy.RequireRole("Cliente"));
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
builder.Services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();

var app = builder.Build();


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapCrearPedidoEndPont();
app.MapPlanesVentaEndPoints();


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