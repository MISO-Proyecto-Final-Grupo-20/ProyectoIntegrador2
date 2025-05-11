using System.Text.Json.Serialization;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Ventas.API.Entidades;

public class Pedido
{
   
    private Pedido() { }
    
    public Pedido(int idCliente, DateTime fechaCreacion, ProductoPedido[] productosPedidos, string nombreCliente, string direccionEntrega, int? idVendedor, string? nombreVendedor)
    {
        IdCliente = idCliente;
        FechaCreacion = fechaCreacion;
        NombreCliente = nombreCliente;
        DireccionEntrega = direccionEntrega;
        IdVendedor = idVendedor;
        NombreVendedor = nombreVendedor;
        ProductosPedidos = productosPedidos.ToList();
    }
    
    public Pedido(SolicitudDePedido solicitud, List<InformacionPoducto> informacionProductos, InformacionCliente informacionCliente, InformacionVendedor? informacionVendedor)
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
        
    
        IdCliente = solicitud.IdCliente;
        NombreCliente = informacionCliente.NombreCliente;
        DireccionEntrega = informacionCliente.LugarEntrega;
        IdVendedor = informacionVendedor?.Id;
        NombreVendedor = informacionVendedor?.Nombre;
        FechaCreacion = solicitud.FechaCreacion;
        ProductosPedidos = productosPedidos.ToList();
    }
    public int Id { get; private set; }
    public int IdCliente { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public string NombreCliente { get; private set; }
    public string DireccionEntrega { get; private set; }
    public int? IdVendedor { get; private set; }
    public string? NombreVendedor { get; private set; }
    public List<ProductoPedido> ProductosPedidos { get; private set; }

    public PedidoResponse ConvertirAResponse()
    {
        return new PedidoResponse(Id, IdCliente, FechaCreacion, FechaCreacion.AddDays(3), DireccionEntrega, EstadoPedido.Pendiente, ProductosPedidos.Select(x => x.ConvertirAResponse()).ToArray(), ProductosPedidos.Sum(x => x.Precio * x.Cantidad), NombreCliente);
    }
    
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
    public int IdPedido { get; private set; }
    public int IdProducto { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }
    public bool TieneInventario { get; private set; }
    public string? Codigo { get; private set; }
    public string? Nombre { get; private set; }
    public string? Imagen { get; private set; }
    
    public ProductoPedidoResponse ConvertirAResponse()
    {
        return new ProductoPedidoResponse(IdProducto, Cantidad, Precio, Codigo, Nombre, Imagen);
    }
}


