using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.Endpoints;
using StoreFlow.Usuarios.API.Infraestructura;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var claveSecreta = "EstaEsUnaClaveSuperSecretaDe32Caracteres!";


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta))
        };
    });


// Registrar el contexto de la base de datos

builder.Services.AddDbContext<UsuariosDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<ProveedorToken>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapUsuariosEndpoints();

//Aplicar migraciones
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UsuariosDbContext>();
    db.Database.Migrate();
}

app.Run();


public partial class Program { }

