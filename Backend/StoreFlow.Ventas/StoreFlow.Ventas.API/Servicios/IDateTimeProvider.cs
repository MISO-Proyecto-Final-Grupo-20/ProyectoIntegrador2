namespace StoreFlow.Ventas.API.Servicios;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}