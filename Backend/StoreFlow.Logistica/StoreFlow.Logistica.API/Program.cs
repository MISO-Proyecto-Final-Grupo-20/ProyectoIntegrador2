using StoreFlow.Compartidos.Core.Infraestructura;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("CONNECTION_STRING_LOGISTICA");

builder.Services.ConfigurarAutenticacion();
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("SoloUsuariosCcp", policy =>
        policy.RequireRole("UsuarioCcp"));
});

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

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}