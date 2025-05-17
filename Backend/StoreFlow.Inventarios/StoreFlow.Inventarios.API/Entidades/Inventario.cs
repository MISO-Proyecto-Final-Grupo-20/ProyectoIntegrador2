namespace StoreFlow.Inventarios.API.Entidades;

public class Inventario
{
    private Inventario()
    {
    }

    public Inventario(int idProducto, int idBodega, int cantidad)
    {
        IdProducto = idProducto;
        IdBodega = idBodega;
        Cantidad = cantidad;
    }

    public int IdProducto { get; private set; }
    public int IdBodega { get; private set; }
    public int Cantidad { get; set; }

    public Bodega Bodega { get; private set; } = default!;
}