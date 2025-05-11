using System.IdentityModel.Tokens.Jwt;
using StoreFlow.Usuarios.API.Servicios;

namespace StoreFlow.Usuarios.API.Endpoints;

public static class RutasVendedoresEndPoints
{
    public static void MapRutasVendedores(this IEndpointRouteBuilder app)
    {
        app.MapGet("/rutasAsignadas", (HttpContext httpContext, IUsuariosServicios usuariosServicios) =>
        {
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var idVendedor = int.Parse(jwtToken.Claims.First(c => c.Type == "idUsuario").Value);

            var direccionBodega = "Calle 100 # 47-50";
            
            var rutaAsignada = usuariosServicios.ObtenerRutaAsignada(idVendedor, direccionBodega);

            return Results.Ok(rutaAsignada);


        }).RequireAuthorization("Vendedor");
    }

    
}