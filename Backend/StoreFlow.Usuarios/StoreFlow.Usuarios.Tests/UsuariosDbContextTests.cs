using Microsoft.EntityFrameworkCore;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;

namespace StoreFlow.Usuarios.Tests;

public class UsuariosDbContextTests
{
    [Fact]
    public void Agregar_UsuarioCliente()
    {
        var opciones = new DbContextOptionsBuilder<UsuariosDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var contexto = new UsuariosDbContext(opciones);
        
        contexto.CrearUsuarioCliente(new CrearClienteRequest("cliente", "correo@correo.com", "Direccion", "12345678" ));

        var usuarioCreado = contexto.Usuarios.First();
        
        Assert.Equal("cliente", usuarioCreado.NombreCompleto);
        Assert.Equal("correo@correo.com", usuarioCreado.CorreoElectronico);
        Assert.Equal("Direccion", usuarioCreado.Direccion);
        Assert.Equal("12345678", usuarioCreado.Contrasena);
        Assert.Equal(TiposUsuarios.Cliente, usuarioCreado.TipoUsuario);
        Assert.Equal(1, usuarioCreado.Id);
    }
}