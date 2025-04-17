using Microsoft.EntityFrameworkCore;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;

namespace StoreFlow.Usuarios.API.Datos;

public class UsuariosDbContext(DbContextOptions<UsuariosDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public void CrearUsuarioCliente(CrearClienteRequest clienteRequest)
    {
        LanzarExcepcionSiCorreoEstaRepetido(clienteRequest.Correo);


        var usuario = new Usuario()
        {
            CorreoElectronico = clienteRequest.Correo!,
            Contrasena = clienteRequest.Contrasena!,
            TipoUsuario = TiposUsuarios.Cliente,
            NombreCompleto = clienteRequest.Nombre!,
            Direccion = clienteRequest.Direccion
        };
        
        Usuarios.Add(usuario);
        SaveChanges();
    }
    
    public void CrearUsuarioVendedor(CrearVendedorRequest crearVendedorRequest)
    {
        LanzarExcepcionSiCorreoEstaRepetido(crearVendedorRequest.Correo);
        
        var usuario = new Usuario()
        {
            CorreoElectronico = crearVendedorRequest.Correo!,
            Contrasena = crearVendedorRequest.Contrasena!,
            TipoUsuario = TiposUsuarios.Vendedor,
            NombreCompleto = crearVendedorRequest.Nombre!,
        };
        
        Usuarios.Add(usuario);
        SaveChanges();
    }

    private void LanzarExcepcionSiCorreoEstaRepetido(string? usuarioCorreoElectronico)
    {
        var correoRepetido = Usuarios
            .Any(u => u.CorreoElectronico == usuarioCorreoElectronico);

        if (correoRepetido)
            throw new UsuarioConCorreoRepetidoException(usuarioCorreoElectronico!);
    }

    
}

public class UsuarioConCorreoRepetidoException(string correo) : Exception(string.Format(UsuariosResources.ElCorreoYaEstaRegistrado, correo));