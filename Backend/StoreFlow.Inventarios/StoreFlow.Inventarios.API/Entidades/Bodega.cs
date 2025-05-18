namespace StoreFlow.Inventarios.API.Entidades;

public class Bodega
{
    private Bodega()
    {
    }

    public Bodega(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }

    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;

    public List<Inventario> Inventarios { get; private set; } = [];
}