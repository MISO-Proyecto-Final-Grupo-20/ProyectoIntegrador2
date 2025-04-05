using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;
using System.Net.Http.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace StoreFlow.Usuarios.Tests
{
    public class LoginEndpointTests
    {
        [Fact]
        public async Task Login_Exitoso_Retorna200Ok()
        {
            // ARRANGE
            var app = TestApplicationFactory.Create();
            await app.StartAsync();
            var client = app.GetTestClient();
            

            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<UsuariosDbContext>();

            db.Usuarios.Add(new Usuario
            {
                CorreoElectronico = "test@correo.com",
                Contrasena = "123456",
                NombreCompleto = "Test User",
                TipoUsuario = TiposUsuarios.Cliente
            });

            await db.SaveChangesAsync();
            

            var loginRequest = new UsuarioLoginRequest("test@correo.com", "123456");

            // ACT
            var response = await client.PostAsJsonAsync("/login", loginRequest);

            // ASSERT
            response.EnsureSuccessStatusCode();
            var tokenString = await response.Content.ReadAsStringAsync();

            tokenString = tokenString.Trim('"'); // Eliminar comillas dobles

            Assert.False(string.IsNullOrWhiteSpace(tokenString), "El token no debe ser vacío");

            var handler = new JsonWebTokenHandler();
            var token =  handler.ReadJsonWebToken(tokenString);

            var rolClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            Assert.False(string.IsNullOrEmpty(rolClaim),"El token debe tener el claim rol");
            Assert.Equal("Cliente", rolClaim);

        }

        [Fact]
        public async Task Login_Invalido_Retorna401Unauthorized()
        {
            // ARRANGE
            var app = TestApplicationFactory.Create();
            await app.StartAsync();
            var client = app.GetTestClient();

            // No se inserta ningún usuario

            var loginRequest = new UsuarioLoginRequest("falso@correo.com", "incorrecta");

            // ACT
            var response = await client.PostAsJsonAsync("/login", loginRequest);

            // ASSERT
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);


            await app.StopAsync();
        }
    }
}
