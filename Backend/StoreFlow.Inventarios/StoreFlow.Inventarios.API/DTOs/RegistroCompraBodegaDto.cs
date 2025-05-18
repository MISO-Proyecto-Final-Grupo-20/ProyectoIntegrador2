namespace StoreFlow.Inventarios.API.DTOs;

public record ProductoAComprarDto(int Id, int Cantidad);

public record RegistroCompraBodegaDto
{
    public int Bodega { get; init; }
    public int Fabricante { get; init; }
    public List<ProductoAComprarDto> Productos { get; init; } = [];
}