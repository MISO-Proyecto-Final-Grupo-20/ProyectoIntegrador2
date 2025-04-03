using Microsoft.EntityFrameworkCore;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);


// Registrar el contexto de la base de datos

builder.Services.AddDbContext<UsuariosDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapUsuariosEndpoints();

app.Run();


public partial class Program { }

