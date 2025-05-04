using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;

namespace StoreFlow.Usuarios.API.Endpoints;

public static class CrearVendedorEndpoints
{
    public static void MapCrearVendedorEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/vendedor", (CrearVendedorRequest crearVendedorDto, UsuariosDbContext db) =>
        {
            try
            {
                crearVendedorDto.Validar();
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(e.Message);
            }

            try
            {
                db.CrearUsuarioVendedor(crearVendedorDto);
                return Results.Created();
            }
            catch (UsuarioConCorreoRepetidoException e)
            {
                return Results.BadRequest(e.Message);
            }

        }).RequireAuthorization("SoloUsuariosCcp");
        
        app.MapGet("/vendedores", async (UsuariosDbContext db) =>
        {
            var vendedores = await db.ObtenerVendedoresAsync();
            return Results.Ok(vendedores);
            
        }).RequireAuthorization("SoloUsuariosCcp");
    }
}