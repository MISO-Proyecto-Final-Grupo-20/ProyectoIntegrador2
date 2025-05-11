using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Infraestructura;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.EndPoints;
using StoreFlow.Ventas.API.Servicios;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Azure;
using Azure.AI.OpenAI;

var builder = WebApplication.CreateBuilder(args);

var connectionString = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("CONNECTION_STRING_VENTAS");

var endpoint = new Uri(EnvironmentUtilidades.ObtenerVariableEntornoRequerida("AZURE_OPENAI_ENDPOINT"));
var key = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("AZURE_OPENAI_KEY");
var deployment = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("AZURE_OPENAI_DEPLOYMENT");

Console.WriteLine(endpoint);
Console.WriteLine(key);
Console.WriteLine(deployment);

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
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddSingleton(new AzureOpenAIClient(endpoint, new AzureKeyCredential(key)));
builder.Services.AddSingleton(provider =>
{
    var client = provider.GetRequiredService<AzureOpenAIClient>();
    return client.GetChatClient(deployment);
});
builder.Services.AddScoped<IProcesadorDeVideo, ProcesadorDeVideo>();
builder.Services.AddScoped<IGeneradorDeRecomendacion, GeneradorDeRecomendacion>();
builder.Services.AddHostedService<ProcesadorDeVideosBackgroundService>();

var app = builder.Build();


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapCrearPedidoEndPont();
app.MapPlanesDeVentasEndPoints();
app.MapVisitasEndPoints();


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