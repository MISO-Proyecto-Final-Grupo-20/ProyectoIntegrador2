namespace StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;

public record InformacionProductoObtenida(Guid IdProceso, List<InformacionPoducto> InformacionProductos);

public record InformacionPoducto(
    int Id,
    string Imagen,
    string Nombre,
    string Codigo,
    decimal Precio);
