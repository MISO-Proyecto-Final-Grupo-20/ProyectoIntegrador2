namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;

public record ObtenerInformacionClienteYVendedor(Guid IdProceso, int IdCliente, int? IdVendedor);