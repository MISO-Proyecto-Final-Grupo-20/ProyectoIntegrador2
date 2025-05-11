namespace StoreFlow.Ventas.API.Entidades;

public class Video
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;
    public string? Recomendacion { get; set; }

    public string? NombreOriginal { get; set; }

    public long TamanioBytes { get; set; }

    public EstadoProcesamiento Estado { get; set; } = EstadoProcesamiento.Pendiente;

    public int VisitaId { get; set; }
    public Visita Visita { get; set; } = null!;
}