using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace StoreFlow.Usuarios.API.Infraestructura;

public sealed class ProveedorToken
{
    private readonly string _jwtSecret;

    public ProveedorToken()
    {
        _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")!;
        if (string.IsNullOrEmpty(_jwtSecret))
        {
            throw new InvalidOperationException("La variable de entorno 'JWT_SECRET' no está definida.");
        }
    }

    public string ObtenerToken(string correoElectronico, string rol, int idUsuario)
    {
        var llaveSeguridad = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSecret));
        var credenciales = new SigningCredentials(llaveSeguridad, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("correo", correoElectronico),
                new Claim(  ClaimTypes.Role, rol),
                new Claim("idUsuario", idUsuario.ToString()),
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credenciales
        };

        var tokenHandler = new JsonWebTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }
}