namespace StoreFlow.Ventas.API.Servicios;

public interface IProcesadorDeVideo
{
    Task ProcesarVideoAsync(int visitaId);
}