namespace StoreFlow.Ventas.API;

public class Resultado<T>
{
    public bool EsExitoso { get; }
    public T? Valor { get; }
    public ErrorDeNegocio? Error { get; }

    private Resultado(bool exito, T? valor, ErrorDeNegocio? error)
    {
        EsExitoso = exito;
        Valor = valor;
        Error = error;
    }

    public static Resultado<T> Exito(T valor) => new(true, valor, null);
    public static Resultado<T> Falla(ErrorDeNegocio error) => new(false, default, error);
}