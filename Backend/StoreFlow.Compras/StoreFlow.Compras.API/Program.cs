using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.Endpoints;
using StoreFlow.Compras.API.Servicios;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_COMPRAS");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La variable de entorno 'CONNECTION_STRING_COMPRAS' no está definida.");
}

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new InvalidOperationException("La variable de entorno 'JWT_SECRET' no está definida.");
}


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
            RoleClaimType = ClaimTypes.Role,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

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