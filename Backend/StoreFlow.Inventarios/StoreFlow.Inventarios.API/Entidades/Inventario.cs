namespace StoreFlow.Inventarios.API.Entidades;

public class Inventario
{
    private Inventario() { }
    
    public Inventario(int idProducto, int cantidad)
    {
        IdProducto = idProducto;
        Cantidad = cantidad;
    }

    public int IdProducto { get; private set; }
    public int Cantidad { get; set; }
}