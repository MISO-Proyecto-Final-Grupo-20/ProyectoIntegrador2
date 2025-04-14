using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Infraestructura;

namespace StoreFlow.Usuarios.API.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (
            HttpContext httpContext,
            UsuariosDbContext db, ProveedorToken proveedorToken) =>
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new UsuarioLoginRequestConverter());

            var loginDto = await JsonSerializer.DeserializeAsync<UsuarioLoginRequest>(httpContext.Request.Body, options);

                
            var usuario = await db.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.CorreoElectronico == loginDto.DatosIngreso.Correo &&
                    u.Contrasena == loginDto.DatosIngreso.Contrasena); 

            if (usuario is null)
                return Results.Unauthorized();
                
            if (usuario.TipoUsuario.ToString().ToLower() != loginDto.TipoCategoria.ToLower())
                return Results.Unauthorized();

            string token = proveedorToken.ObtenerToken(usuario.CorreoElectronico,usuario.TipoUsuario.ToString());

            return Results.Ok(new UsuarioLoginResponse(token));
        });
    }
    
    
    
    
}

public static class CrearClienteEndpoints
{
    public static void MapCrearClienteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login/cliente", (CrearClienteRequest crearClienteDto, UsuariosDbContext db) =>
        {
            try
            {
                crearClienteDto.Validar();
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(e.Message);
            }

            try
            {
                db.CrearUsuarioCliente(crearClienteDto);
                return Results.Created();
            }
            catch (UsuarioConCorreoRepetidoException e)
            {
                return Results.BadRequest(e.Message);
            }

        });
    }
}
