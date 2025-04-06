using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;
using System.Net.Http.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;

namespace StoreFlow.Usuarios.Tests
{
    public class LoginEndpointTests : IAsyncLifetime
    {
        private HttpClient _client = null!;
        private WebApplication _app = null!;

        public async Task InitializeAsync()
        {
            _app = TestApplicationFactory.Create();
            await _app.StartAsync();
            _client = _app.GetTestClient();
            await CrearUsuarioAsync();
        }
        
        [Theory]
        [InlineData("test@correo.com", "123456", "cliente", "Cliente")]
        [InlineData("vendedor@correo.com", "123456", "vendedor", "Vendedor")]
        public async Task Login_Exitoso_Retorna200Ok(string correo, string contrasena, string rol, string rolEsperado)
        {
            // ARRANGE
            var loginRequest = new UsuarioLoginRequest(new DatosIngreso(correo, contrasena), rol);

            // ACT
            var response = await _client.PostAsJsonAsync("/login", loginRequest);

            // ASSERT
            response.EnsureSuccessStatusCode();
            var loginResponse = await response.Content.ReadFromJsonAsync<UsuarioLoginResponse>();
            var tokenString = loginResponse?.Token.Trim('"'); 

            Assert.False(string.IsNullOrWhiteSpace(tokenString), "El token no debe ser vacío");

            var handler = new JsonWebTokenHandler();
            var token =  handler.ReadJsonWebToken(tokenString);

            var rolClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            Assert.False(string.IsNullOrEmpty(rolClaim),"El token debe tener el claim rol");
            Assert.Equal(rolEsperado, rolClaim);

        }

        

        [Fact]
        public async Task Login_con_usuario_inexistente_Retorna_401Unauthorized()
        {
            // ARRANGE
            var loginRequest = new UsuarioLoginRequest(new DatosIngreso("falso@correo.com", "incorrecta"), "cliente");

            // ACT
            var response = await _client.PostAsJsonAsync("/login", loginRequest);

            // ASSERT
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        public async Task Login_con_contrasena_incorrecta_Retorna_401Unauthorized()
        {
            // ARRANGE
            var loginRequest = new UsuarioLoginRequest(new DatosIngreso("test@correo.com", "incorrecta"), "cliente");

            // ACT
            var response = await _client.PostAsJsonAsync("/login", loginRequest);

            // ASSERT
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);

        }
        
        [Fact]
        public async Task Login_con_tipoUsuario_incorrecto_Retorna_401Unauthorized()
        {
            // ARRANGE
            var loginRequest = new UsuarioLoginRequest(new DatosIngreso("test@correo.com", "123456"), "vendedor");

            // ACT
            var response = await _client.PostAsJsonAsync("/login", loginRequest);

            // ASSERT
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);

        }

        public async  Task DisposeAsync()
        {
             await _app.StopAsync();
        }
        
        private async Task CrearUsuarioAsync()
        {
            using var scope = _app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<UsuariosDbContext>();

            db.Usuarios.Add(new Usuario
            {
                CorreoElectronico = "test@correo.com",
                Contrasena = "123456",
                NombreCompleto = "Test User",
                TipoUsuario = TiposUsuarios.Cliente
            });
            
            db.Usuarios.Add(new Usuario
            {
                CorreoElectronico = "vendedor@correo.com",
                Contrasena = "123456",
                NombreCompleto = "Vendedor User",
                TipoUsuario = TiposUsuarios.Vendedor
            });

            await db.SaveChangesAsync();
        }
    }
}
