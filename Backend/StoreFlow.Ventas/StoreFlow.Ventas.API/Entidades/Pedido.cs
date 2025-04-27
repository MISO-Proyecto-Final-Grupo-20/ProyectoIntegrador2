using StoreFlow.Ventas.API.EndPoints;

namespace StoreFlow.Ventas.API.Entidades;

public class Pedido
{
    public Pedido(int id, int idCliente, DateTime fechaCreacion, ProductoPedido[] productosPedidos)
    {
        Id = id;
        IdCliente = idCliente;
        FechaCreacion = fechaCreacion;
        ProductosPedidos = productosPedidos;
    }
    public int Id { get; private set; }
    public int IdCliente { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public ProductoPedido[] ProductosPedidos { get; private set; }
    
}

public class ProductoPedido
{
    public ProductoPedido(int idProducto, int cantidad, decimal precio)
    {
        IdProducto = idProducto;
        Cantidad = cantidad;
        Precio = precio;
    }
    public int IdPedido { get; private set; }
    public int IdProducto { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }
}