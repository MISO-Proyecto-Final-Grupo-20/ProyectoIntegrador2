using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StoreFlow.Ventas.Tests;

public static class GeneradorTokenPruebas
{
    public static readonly string ClavePruebas = "q9zM3u9Y5xUq4OEt5nq3P9+0uOaIxQeH+dE68Z8+WBA=";

    public static string GenerarTokenUsuarioCcp(string correo = "admin@correo.com")
    {
        return GenerarToken(correo, "UsuarioCcp");
    }

    public static string GenerarTokenVendedor(string correo = "vendedor@correo.com")
    {
        return GenerarToken(correo,"Vendedor");
    }

    private static string GenerarToken(string correo, string rol)
    {
        var claims = new[]
        {
            new Claim("correo", correo),
            new Claim(ClaimTypes.Role, rol)
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