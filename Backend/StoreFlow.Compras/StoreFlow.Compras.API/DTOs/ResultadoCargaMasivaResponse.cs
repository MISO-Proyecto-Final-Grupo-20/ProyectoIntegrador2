namespace StoreFlow.Compras.API.DTOs;

public record ResultadoCargaMasivaResponse
{
    public List<string> Errores { get; init; } = new();
    public List<ProductoCargadoDto> Productos { get; init; } = new();
}

public record ProductoCargadoDto
{
    public string Nombre { get; init; } = null!;
    public string Codigo { get; init; } = null!;
    public decimal Precio { get; init; }
    public string Imagen { get; init; } = null!;
    public FabricanteDto FabricanteAsociado { get; init; } = null!;
}