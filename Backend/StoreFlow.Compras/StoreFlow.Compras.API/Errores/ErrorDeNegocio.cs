namespace StoreFlow.Compras.API.Errores;

public abstract class ErrorDeNegocio
{
    public abstract string Mensaje { get; }
    public virtual string Categoria => "ERROR_NEGOCIO";
}