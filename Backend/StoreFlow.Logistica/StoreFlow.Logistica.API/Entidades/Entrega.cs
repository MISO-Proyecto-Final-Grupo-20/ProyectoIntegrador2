using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Logistica.API.Entidades;

public class Entrega
{
   
    private Entrega() { }
    
    public Entrega(PedidoResponse pedido)
    {
        IdPedido = pedido.Numero;
        IdCliente = pedido.IdCliente;
        NombreCliente = pedido.NombreCliente;
        DireccionEntrega = pedido.LugarEntrega;
        FechaCreacion = pedido.FechaRegistro;
        FechaProgramadaEntrega = pedido.FechaEntrega;
        ProductosPedidos = pedido.Productos
        .Select( p => new ProductoPedido(
            p.Id,p.Cantidad, p.Precio,  p.Codigo, p.Nombre, p.Imagen
            
        )).ToList();
    }
    public int Id { get; private set; }
    public int IdCliente { get; private set; }
    public int IdPedido { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaProgramadaEntrega { get; private set; }
    public string NombreCliente { get; private set; }
    public string DireccionEntrega { get; private set; }
    public List<ProductoPedido> ProductosPedidos { get; private set; }
    
}

public class ProductoPedido
{
    private ProductoPedido()
    {
    }
    public ProductoPedido(int idProducto, int cantidad, decimal precio,  string? codigo, string? nombre, string? imagen)
    {
        IdProducto = idProducto;
        Cantidad = cantidad;
        Precio = precio;
        Codigo = codigo;
        Nombre = nombre;
        Imagen = imagen;
    }
    public int IdEntrega { get; private set; }
    public int IdProducto { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }
    public string? Codigo { get; private set; }
    public string? Nombre { get; private set; }
    public string? Imagen { get; private set; }
    
    
}
