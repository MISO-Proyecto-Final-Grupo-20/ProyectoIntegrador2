using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;
using StoreFlow.Usuarios.API.Servicios;

namespace StoreFlow.Usuarios.Tests;

public class UsuariosDbContextTests
{
    private readonly UsuariosDbContext _contexto;
    private readonly UsuariosServicios _usuariosServicios;

    public UsuariosDbContextTests()
    {
        var opciones = new DbContextOptionsBuilder<UsuariosDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _contexto = new UsuariosDbContext(opciones);
        _usuariosServicios = new UsuariosServicios(_contexto);
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

    [Fact]
    public void ObtenerInformacionClienteYVendedor_SinVendedor()
    {
        _contexto.CrearUsuarioCliente(new CrearClienteRequest("cliente", "correo@correo.com", "Direccion", "12345678" ));
        _contexto.CrearUsuarioVendedor(new CrearVendedorRequest("vendedor", "vendedor@correo.com", "12345678" ));

        var informacionClienteYVendedor = _usuariosServicios.ObtenerInformacionClienteYVendedor(1, null);
        Assert.Null(informacionClienteYVendedor.informacionVendedor);
        Assert.Equal(new InformacionCliente(1, "Direccion", "cliente"), informacionClienteYVendedor.informacionCliente);
    }

    [Fact]
    public void ObtenerInformacionClienteYVendedor_ConVendedor()
    {
        _contexto.CrearUsuarioCliente(new CrearClienteRequest("cliente", "correo@correo.com", "Direccion", "12345678"));
        _contexto.CrearUsuarioVendedor(new CrearVendedorRequest("vendedor", "vendedor@correo.com", "12345678"));

        var informacionClienteYVendedor = _usuariosServicios.ObtenerInformacionClienteYVendedor(1, 2);
        
        Assert.Equal(new InformacionCliente(1, "Direccion", "cliente"), informacionClienteYVendedor.informacionCliente);
        Assert.Equal(new InformacionVendedor(2, "vendedor"), informacionClienteYVendedor.informacionVendedor);
    }
    
    [Fact]
    public void ObtenerInformacionClienteYVendedor_SinCliente()
    {
        _contexto.CrearUsuarioVendedor(new CrearVendedorRequest("vendedor", "vendedor@correo.com", "12345678"));

        var informacionClienteYVendedor = _usuariosServicios.ObtenerInformacionClienteYVendedor(4, 1);
        
        Assert.Equal(new InformacionCliente(4, "Sin Dirección registrada.", "Sin Nombre registrado."), informacionClienteYVendedor.informacionCliente);
        Assert.Equal(new InformacionVendedor(1, "vendedor"), informacionClienteYVendedor.informacionVendedor);
    }


}