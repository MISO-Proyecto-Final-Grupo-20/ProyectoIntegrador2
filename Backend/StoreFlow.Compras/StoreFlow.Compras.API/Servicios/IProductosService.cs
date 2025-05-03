using StoreFlow.Compras.API.Comunes;
using StoreFlow.Compras.API.DTOs;

namespace StoreFlow.Compras.API.Servicios;

public interface IProductosService
{
    Task<Resultado<CrearProductoResponse>> CrearProductoAsync(CrearProductoRequest request);
    Task<ProductoResponse[]> ObtenerProductosAsync();
    Task<ResultadoCargaMasivaResponse> ValidarProductosMasivoAsync(IFormFile archivoCsv);
    Task GuardarProductosMasivosAsync(List<RegistrarProducto> productos);
}