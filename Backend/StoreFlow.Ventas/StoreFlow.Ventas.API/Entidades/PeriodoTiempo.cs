namespace StoreFlow.Ventas.API.Entidades;

public class PeriodoTiempo
{
    private PeriodoTiempo()
    {
    }

    public PeriodoTiempo(string nombre)
    {
        Nombre = nombre;
    }

    public int Id { get; private set; }
    public string Nombre { get; private set; } = null!;
    public List<PlanVenta> PlanesVenta { get; private set; } = new();
}