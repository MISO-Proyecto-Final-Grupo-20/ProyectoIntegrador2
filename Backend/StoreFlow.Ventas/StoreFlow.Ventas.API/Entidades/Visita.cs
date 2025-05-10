namespace StoreFlow.Ventas.API.Entidades;

public class Visita
{
    public int Id { get; set; }
    public int IdVendedor { get; set; }
    public int IdCliente { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public Video? Video { get; set; }
}