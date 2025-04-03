using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;

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
            var token = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(token), "El token no debe ser vacío");
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
