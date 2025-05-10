namespace StoreFlow.Ventas.API.Servicios;

public interface IBlobStorageService
{
    Task<string> SubirVideoAsync(IFormFile archivo, string nombreArchivo);
}