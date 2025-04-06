using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Infraestructura;

namespace StoreFlow.Usuarios.API.Endpoints
{
    public static class UsuariosEndpoints
    {
        public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (
                [FromBody] UsuarioLoginRequest loginDto,
                UsuariosDbContext db, ProveedorToken proveedorToken) =>
            {
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
}
