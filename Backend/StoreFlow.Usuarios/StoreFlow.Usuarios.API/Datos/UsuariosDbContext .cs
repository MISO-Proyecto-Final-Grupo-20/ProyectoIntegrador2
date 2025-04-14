using Microsoft.EntityFrameworkCore;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;

namespace StoreFlow.Usuarios.API.Datos;

public class UsuariosDbContext(DbContextOptions<UsuariosDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public void CrearUsuarioCliente(CrearClienteRequest clienteRequest)
    {
        var correoRepetido = Usuarios
            .Any(u => u.CorreoElectronico == clienteRequest.Correo);
        
        if (correoRepetido)
            throw new UsuarioConCorreoRepetidoException(clienteRequest.Correo);
        
        
        var usuario = new Usuario()
        {
            CorreoElectronico = clienteRequest.Correo,
            Contrasena = clienteRequest.Contrasena,
            TipoUsuario = TiposUsuarios.Cliente,
            NombreCompleto = clienteRequest.Nombre,
            Direccion = clienteRequest.Direccion
        };
        
        Usuarios.Add(usuario);
        SaveChanges();
    }
}

public class UsuarioConCorreoRepetidoException(string correo) : Exception(string.Format(UsuariosResources.ElCorreoYaEstaRegistrado, correo));