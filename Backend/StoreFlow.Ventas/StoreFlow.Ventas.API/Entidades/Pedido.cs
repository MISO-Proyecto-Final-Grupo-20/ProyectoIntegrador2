using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Ventas.API.Entidades;

public class Pedido
{
   
    private Pedido() { }
    
    public Pedido(int idCliente, DateTime fechaCreacion, ProductoPedido[] productosPedidos)
    {
        IdCliente = idCliente;
        FechaCreacion = fechaCreacion;
        ProductosPedidos = productosPedidos;
    }
    
    public Pedido(SolicitudDePedido solicitud)
    {
        var productosPedidos = solicitud
            .productosSolicitados
            .Select(p => new ProductoPedido(p.Id, p.Cantidad, p.Precio, p.TieneInventario))
            .ToArray();

        IdCliente = solicitud.IdCliente;
        FechaCreacion = solicitud.FechaCreacion;
        ProductosPedidos = productosPedidos;
    }
    public int Id { get; private set; }
    public int IdCliente { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public ProductoPedido[] ProductosPedidos { get; private set; }
    
}

public class ProductoPedido
{
    public ProductoPedido(int idProducto, int cantidad, decimal precio, bool tieneInventario)
    {
        IdProducto = idProducto;
        Cantidad = cantidad;
        Precio = precio;
        TieneInventario = tieneInventario;
    }
    public int IdPedido { get; private set; }
    public int IdProducto { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }
    public bool TieneInventario { get; private set; }
}