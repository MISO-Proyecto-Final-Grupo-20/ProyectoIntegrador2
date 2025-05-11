using System.IdentityModel.Tokens.Jwt;

namespace StoreFlow.Ventas.API.EndPoints;

public static class UtilidadesEndPoints
{
    public static int RecuperarIdUsuarioToken(HttpContext httpContext)
    {
        var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var idUsuario = int.Parse(jwtToken.Claims.First(c => c.Type == "idUsuario").Value);
        return idUsuario;
    }
}