namespace StoreFlow.Ventas.API.Servicios;

public interface IGeneradorDeRecomendacion
{
    Task<string> GenerarAsync(string prompt);
}