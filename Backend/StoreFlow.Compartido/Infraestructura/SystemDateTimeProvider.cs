namespace StoreFlow.Compartidos.Core.Infraestructura;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}