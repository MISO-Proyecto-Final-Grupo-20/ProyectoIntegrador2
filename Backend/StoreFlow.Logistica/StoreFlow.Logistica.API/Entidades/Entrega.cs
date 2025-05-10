using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Logistica.API.Entidades;

public class Entrega
{
   
    private Entrega() { }
    
    public Entrega(SolicitudDePedido solicitud, List<InformacionPoducto> informacionProductos, InformacionCliente informacionCliente, InformacionVendedor? informacionVendedor, int idPedido)
    {
        
        var productosPedidos = from productoSolicitado in solicitud.ProductosSolicitados
            join infoProducto in informacionProductos
                on productoSolicitado.Id equals infoProducto.Id into productoJoin
            from infoProducto in productoJoin.DefaultIfEmpty()
            select new ProductoPedido(
                productoSolicitado.Id,
                productoSolicitado.Cantidad,
                productoSolicitado.Precio,
                productoSolicitado.TieneInventario,
                infoProducto?.Codigo ?? string.Empty,
                infoProducto?.Nombre ?? string.Empty,
                infoProducto?.Imagen ?? string.Empty
            );

        IdPedido = idPedido;
        IdCliente = solicitud.IdCliente;
        NombreCliente = informacionCliente.NombreCliente;
        DireccionEntrega = informacionCliente.LugarEntrega;
        IdVendedor = informacionVendedor?.Id;
        NombreVendedor = informacionVendedor?.Nombre;
        FechaCreacion = solicitud.FechaCreacion;
        FechaProgramadaEntrega = solicitud.FechaCreacion.AddDays(3);
        ProductosPedidos = productosPedidos.ToList();
    }
    public int Id { get; private set; }
    public int IdCliente { get; private set; }
    public int IdPedido { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaProgramadaEntrega { get; private set; }
    public string NombreCliente { get; private set; }
    public string DireccionEntrega { get; private set; }
    public int? IdVendedor { get; private set; }
    public string? NombreVendedor { get; private set; }
    public List<ProductoPedido> ProductosPedidos { get; private set; }

    
    
}

public class ProductoPedido
{
    private ProductoPedido()
    {
    }
    public ProductoPedido(int idProducto, int cantidad, decimal precio, bool tieneInventario, string? codigo, string? nombre, string? imagen)
    {
        IdProducto = idProducto;
        Cantidad = cantidad;
        Precio = precio;
        TieneInventario = tieneInventario;
        Codigo = codigo;
        Nombre = nombre;
        Imagen = imagen;
    }
    public int IdEntrega { get; private set; }
    public int IdProducto { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }
    public bool TieneInventario { get; private set; }
    public string? Codigo { get; private set; }
    public string? Nombre { get; private set; }
    public string? Imagen { get; private set; }
    
    
}
