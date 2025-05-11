using System.IdentityModel.Tokens.Jwt;
using StoreFlow.Logistica.API.Servicios;

namespace StoreFlow.Logistica.API.Endpoints;

public static class EntregasEndpoints
{
    public static void MapEntregasEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/entregasProgramadas",
            async (HttpContext httpContext, IEntregaServicio entregaServicio) =>
            {
                var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var idCliente = int.Parse(jwtToken.Claims.First(c => c.Type == "idUsuario").Value);
                
                var entregasProgramadas = await entregaServicio.ObtenerEntregasClienteAsync(idCliente);
                return Results.Ok(entregasProgramadas);
            }).RequireAuthorization("Cliente");
    }
}