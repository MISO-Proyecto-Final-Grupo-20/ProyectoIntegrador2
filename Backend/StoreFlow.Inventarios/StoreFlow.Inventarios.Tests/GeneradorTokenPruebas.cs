using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StoreFlow.Inventarios.Tests;

public static class GeneradorTokenPruebas
{
    public static readonly string ClavePruebas = "q9zM3u9Y5xUq4OEt5nq3P9+0uOaIxQeH+dE68Z8+WBA=";

    public static string GenerarTokenUsuarioCcp(string correo = "admin@correo.com")
    {
        return GenerarToken(correo, "UsuarioCcp", 1);
    }

    public static string GenerarTokenVendedor(string correo = "vendedor@correo.com")
    {
        return GenerarToken(correo,"Vendedor", 2);
    }
    
    public static string GenerarTokenCliente(string correo = "cliente@correo.com")
    {
        return GenerarToken(correo,"Cliente", 3);
    }

    private static string GenerarToken(string correo, string rol, int idUsuario)
    {
        var claims = new[]
        {
            new Claim("correo", correo),
            new Claim(ClaimTypes.Role, rol),
            new Claim("IdUsuario", idUsuario.ToString()),
        };
        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ClavePruebas));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: credenciales,
            expires: DateTime.UtcNow.AddDays(1)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}