using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
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

   

    public async Task<ClienteResponse[]> ObtenerClientesAsync()
    {
        var clientes = await Usuarios
            .Where(u => u.TipoUsuario == TiposUsuarios.Cliente)
            .OrderBy(c => c.NombreCompleto)
            .ToArrayAsync();
        
        return  clientes
            .Select(c => c.ConvertirAClienteResponse())
            .ToArray();
    }

    private void LanzarExcepcionSiCorreoEstaRepetido(string? usuarioCorreoElectronico)
    {
        var correoRepetido = Usuarios
            .Any(u => u.CorreoElectronico == usuarioCorreoElectronico);

        if (correoRepetido)
            throw new UsuarioConCorreoRepetidoException(usuarioCorreoElectronico!);
    }


    public async Task<VendedorResponse[]> ObtenerVendedoresAsync()
    {
        var vendedores = await Usuarios.Where(u => u.TipoUsuario == TiposUsuarios.Vendedor)
            .OrderBy(c => c.NombreCompleto)
            .ToListAsync();
        
        return vendedores
            .Select(v => v.ConvertirAVendedorResponse())
            .ToArray();
    }
}

public class UsuarioConCorreoRepetidoException(string correo) : Exception(string.Format(UsuariosResources.ElCorreoYaEstaRegistrado, correo));