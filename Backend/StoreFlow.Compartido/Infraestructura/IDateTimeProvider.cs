namespace StoreFlow.Compartidos.Core.Infraestructura;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}