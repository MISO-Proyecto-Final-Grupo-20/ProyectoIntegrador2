namespace StoreFlow.Ventas.API.Entidades;

public class PlanVenta
{
    private PlanVenta()
    {
    }

    public PlanVenta(string nombre, string descripcion, decimal precio, int periodoTiempoId)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        Precio = precio;
        PeriodoTiempoId = periodoTiempoId;
    }

    public int Id { get; private set; }
    public string Nombre { get; private set; } = null!;
    public string Descripcion { get; private set; } = null!;
    public decimal Precio { get; private set; }
    public int PeriodoTiempoId { get; private set; }
    public PeriodoTiempo PeriodoTiempo { get; private set; } = null!;
}