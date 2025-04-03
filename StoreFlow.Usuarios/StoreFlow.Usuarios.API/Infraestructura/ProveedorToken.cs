using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace StoreFlow.Usuarios.API.Infraestructura;

public sealed class ProveedorToken(IConfiguration configuracion)
{
    public string ObtenerToken(string correoElectronico, string contrasena)
    {
        var claveSecreta = "ClaveSuperLargaParaJwtDePrueba12345!"; //  Solo para pruebas

        var llaveSeguridad = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(claveSecreta));
        var credenciales = new SigningCredentials(llaveSeguridad, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Email, correoElectronico),
                new Claim(ClaimTypes.Role, "Usuario")
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credenciales
        };

        var tokenHandler = new JsonWebTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }

}