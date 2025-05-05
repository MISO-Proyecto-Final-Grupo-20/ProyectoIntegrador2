using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;

namespace StoreFlow.Usuarios.API.Endpoints;

public static class CrearClienteEndpoints
{
    public static void MapCrearClienteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/cliente", (CrearClienteRequest crearClienteDto, UsuariosDbContext db) =>
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

        app.MapGet("/clientes", async (UsuariosDbContext db) =>
        {
            var clientes = await db.ObtenerClientesAsync();
            return Results.Ok(clientes);
        });
    }
}