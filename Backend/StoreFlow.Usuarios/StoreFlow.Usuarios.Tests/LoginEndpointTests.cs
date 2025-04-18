﻿using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;
using System.Net.Http.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;

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
            var token = handler.ReadJsonWebToken(tokenString);

            var rolClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            Assert.False(string.IsNullOrEmpty(rolClaim), "El token debe tener el claim rol");
            Assert.Equal(rolEsperado, rolClaim);
        }

        [Theory]
        [InlineData("usuario@correo.com", "123456", "UsuarioCcp")]
        public async Task Login_Exitoso_De_UsuarioCcp_Retorna200Ok(string correo, string contrasena, string rolEsperado)
        {
            // ARRANGE
            var loginRequest = new DatosIngreso(correo, contrasena);

            // ACT
            var response = await _client.PostAsJsonAsync("/login", loginRequest);

            // ASSERT
            response.EnsureSuccessStatusCode();
            var loginResponse = await response.Content.ReadFromJsonAsync<UsuarioLoginResponse>();
            var tokenString = loginResponse?.Token.Trim('"');

            Assert.False(string.IsNullOrWhiteSpace(tokenString), "El token no debe ser vacío");

            var handler = new JsonWebTokenHandler();
            var token = handler.ReadJsonWebToken(tokenString);

            var rolClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            Assert.False(string.IsNullOrEmpty(rolClaim), "El token debe tener el claim rol");
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

        [Fact]
        public async Task CrearCliente_con_informacion_completa_RetornaCreated()
        {
            var request =
                new CrearClienteRequest("nombre usuario", "nuevo_usuario@correo.com", "direccion", "12345678");

            var response = await _client.PostAsJsonAsync("/cliente", request);

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("", "nuevo_usuario@correo.com", "direccion", "12345678", "El nombre es obligatorio")]
        [InlineData("nombre", "", "direccion", "12345678", "El correo es obligatorio")]
        [InlineData("nombre", "nuevo_usuario@correo.com", "direccion", null, "La contraseña es obligatoria")]
        [InlineData("nombre", "nuevo_usuario@correo.com", "direccion", "123", "La contraseña debe tener al menos 8 caracteres")]
        [InlineData("nombre", "nuevo_usuario", "direccion", "123456789", "El correo no tiene un formato válido")]
        [InlineData("nuevo_usuarioaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "nuevo_usuario@correo.com", "direccion", "123456789", "El nombre no puede exceder los 100 caracteres")]
        [InlineData("nombre", "nuevo_usuario@aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.comcom", "direccion", "123456789", "El correo no puede tener más de 100 caracteres")]
        public async Task CrearCliente_con_informacin_incompleta_RetornaBadRequest(string? nombre, string? correo, string? direccion, string? contrasena, string mensajeEsperado)
        {
            var request = new CrearClienteRequest(nombre, correo, direccion, contrasena);

            var response = await _client.PostAsJsonAsync("/cliente", request);
            var responseMessage = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(mensajeEsperado, responseMessage.Trim('"'));
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task CrearCliente_con_correo_Repetido_retorna_badRequest()
        {
            var request =
                new CrearClienteRequest("nombre usuario", "nuevo_usuario@correo.com", "direccion", "12345678");
            _ = await _client.PostAsJsonAsync("/cliente", request);

            var response = await _client.PostAsJsonAsync("/cliente", request);

            var responseMessage = await response.Content.ReadAsStringAsync();
            
            Assert.Equal("El correo nuevo_usuario@correo.com ya está registrado.", responseMessage.Trim('"'));
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        

        public async Task DisposeAsync()
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

            db.Usuarios.Add(new Usuario
            {
                CorreoElectronico = "usuario@correo.com",
                Contrasena = "123456",
                NombreCompleto = "Usuario ccp",
                TipoUsuario = TiposUsuarios.UsuarioCcp
            });

            await db.SaveChangesAsync();
        }
    }
}