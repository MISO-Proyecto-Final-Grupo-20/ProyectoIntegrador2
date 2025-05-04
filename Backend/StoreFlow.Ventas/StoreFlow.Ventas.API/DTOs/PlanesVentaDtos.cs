namespace StoreFlow.Ventas.API.DTOs;

public record PeriodoTiempoDto
{
    // Constructor maps from our domain model to frontend expected properties
    public PeriodoTiempoDto(int id, string nombre)
    {
        this.id = id;
        this.descripcion = nombre; // Map Nombre to descripcion for frontend
    }

    public int id { get; init; }
    public string descripcion { get; init; }
}

public record PlanVentaDto(int Id, string Nombre, string Descripcion, decimal Precio, int PeriodoTiempoId, string NombrePeriodo);

public record CrearPlanVentaDto(string Nombre, string Descripcion, decimal Precio, int PeriodoTiempoId);


public record ActualizarPlanVentaDto(string Nombre, string Descripcion, decimal Precio, int PeriodoTiempoId);