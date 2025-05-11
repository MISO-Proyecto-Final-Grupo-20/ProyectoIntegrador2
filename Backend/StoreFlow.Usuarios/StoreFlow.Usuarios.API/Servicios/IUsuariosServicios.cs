using StoreFlow.Compartidos.Core.Enrutador;
using StoreFlow.Compartidos.Core.Infraestructura;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Usuarios.API.Datos;
using StoreFlow.Usuarios.API.DTOs;
using StoreFlow.Usuarios.API.Entidades;

namespace StoreFlow.Usuarios.API.Servicios;

public interface IUsuariosServicios
{
    (InformacionCliente informacionCliente, InformacionVendedor? informacionVendedor)
        ObtenerInformacionClienteYVendedor(int idCliente, int? idVendedor);

    List<RutaVendedorResponse> ObtenerRutaAsignada(int idVendedor, string direccionBodega);
}

public class UsuariosServicios(UsuariosDbContext usuariosDbContext, IDateTimeProvider dateTimeProvider) : IUsuariosServicios
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
    
    public List<RutaVendedorResponse> ObtenerRutaAsignada(int idVendedor, string direccionBodega)
    {
        var hoy = dateTimeProvider.UtcNow;
        var hoySinHoras = new DateTime(hoy.Year, hoy.Month, hoy.Day);
        var rangoFechas =  Enumerable.Range(-1, 7).Select(d => hoySinHoras.AddDays(d)).ToList();
        
        
        var direccionesClientes = usuariosDbContext
            .Usuarios
            .Where(u => u.TipoUsuario == TiposUsuarios.Cliente)
            .Select(u => new {u.NombreCompleto, u.Direccion,})
            .ToList();

        var direccionesDestino = direccionesClientes.Select(d => d.Direccion).ToList(); 
        var rutas = CalculadoraRutas.CalcularRutas(direccionBodega, direccionesDestino);
        
        var clientesAVisitarPorRutaNorte = (
            from ruta in rutas["Norte"]
            join cliente in direccionesClientes on ruta.Direccion equals cliente.Direccion
            select new {cliente.NombreCompleto, cliente.Direccion}).ToList();

        var tipoRuta = "Sur";
        var clientesAVisitarPorRutaSur = (
            from ruta in rutas[tipoRuta]
            join cliente in direccionesClientes on ruta.Direccion equals cliente.Direccion
            select new {cliente.NombreCompleto, cliente.Direccion}).ToList();
        
        List<RutaVendedorResponse> respuesta = [];
        rangoFechas.ForEach(f =>
        {
            if (idVendedor % 2 == 0)
            {
                if ( f.Day % 2 == 0)
                    respuesta.AddRange(clientesAVisitarPorRutaSur.Select(c =>
                        new RutaVendedorResponse(c.NombreCompleto, c.Direccion, f)));
                else
                    respuesta.AddRange(clientesAVisitarPorRutaNorte.Select(c =>
                        new RutaVendedorResponse(c.NombreCompleto, c.Direccion, f)));
            }
            else
            {
                if ( f.Day % 2 == 0)
                    respuesta.AddRange(clientesAVisitarPorRutaNorte.Select(c =>
                        new RutaVendedorResponse(c.NombreCompleto, c.Direccion, f)));    
                else
                    respuesta.AddRange(clientesAVisitarPorRutaSur.Select(c =>
                    new RutaVendedorResponse(c.NombreCompleto, c.Direccion, f)));    
                
            }
                
        });

        return respuesta;

    }
}