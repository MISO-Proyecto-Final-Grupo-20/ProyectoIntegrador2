using Microsoft.EntityFrameworkCore;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;

namespace StoreFlow.Usuarios.Tests;

public class UsuariosDbContextTests
{
    private UsuariosDbContext _contexto;

    public UsuariosDbContextTests()
    {
        var opciones = new DbContextOptionsBuilder<UsuariosDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _contexto = new UsuariosDbContext(opciones);
    }
    [Fact]
    public void Agregar_UsuarioCliente()
    {
        _contexto.CrearUsuarioCliente(new CrearClienteRequest("cliente", "correo@correo.com", "Direccion", "12345678" ));

        var usuarioCreado = _contexto.Usuarios.First();
        
        Assert.Equal("cliente", usuarioCreado.NombreCompleto);
        Assert.Equal("correo@correo.com", usuarioCreado.CorreoElectronico);
        Assert.Equal("Direccion", usuarioCreado.Direccion);
        Assert.Equal("12345678", usuarioCreado.Contrasena);
        Assert.Equal(TiposUsuarios.Cliente, usuarioCreado.TipoUsuario);
        Assert.Equal(1, usuarioCreado.Id);
    }
    
    
    [Fact]
    public void Agregar_UsuarioVendedor()
    {
        _contexto.CrearUsuarioVendedor(new CrearVendedorRequest("vendedor", "vendedor@correo.com", "12345678" ));

        var usuarioCreado = _contexto.Usuarios.First();
        
        Assert.Equal("vendedor", usuarioCreado.NombreCompleto);
        Assert.Equal("vendedor@correo.com", usuarioCreado.CorreoElectronico);
        Assert.Null(usuarioCreado.Direccion);
        Assert.Equal("12345678", usuarioCreado.Contrasena);
        Assert.Equal(TiposUsuarios.Vendedor, usuarioCreado.TipoUsuario);
        Assert.Equal(1, usuarioCreado.Id);
    }

    
}