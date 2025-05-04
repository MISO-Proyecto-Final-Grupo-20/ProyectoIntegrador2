using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Usuarios.API.Datos;

namespace StoreFlow.Usuarios.API.Servicios;

public interface IUsuariosServicios
{
    (InformacionCliente informacionCliente, InformacionVendedor? informacionVendedor)
        ObtenerInformacionClienteYVendedor(int idCliente, int? idVendedor);
}

public class UsuariosServicios(UsuariosDbContext usuariosDbContext) : IUsuariosServicios
{
    public (InformacionCliente informacionCliente, InformacionVendedor? informacionVendedor)
        ObtenerInformacionClienteYVendedor(int idCliente, int? idVendedor)
    {
        var informacionCliente = usuariosDbContext.Usuarios
            .Where(u => u.Id == idCliente)
            .Select(u => new InformacionCliente(u.Id, u.Direccion ?? "Sin Dirección registrada.", u.NombreCompleto))
            .AsEnumerable()
            .FirstOrDefault(new InformacionCliente(idCliente, "Sin Dirección registrada.", "Sin Nombre registrado."));

        var informacionVendedor = usuariosDbContext.Usuarios.Where(u => u.Id == idVendedor)
            .Select(u => new InformacionVendedor(u.Id, u.NombreCompleto))
            .FirstOrDefault();
        
        return (informacionCliente, informacionVendedor);
    }
}