namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;

public record InformacionClienteYVendedorObtenida(Guid IdProceso, InformacionCliente InformacionCliente, InformacionVendedor? InformacionVendedor);
public record InformacionCliente(int Id, string LugarEntrega, string NombreCliente);
public record InformacionVendedor(int Id, string Nombre);