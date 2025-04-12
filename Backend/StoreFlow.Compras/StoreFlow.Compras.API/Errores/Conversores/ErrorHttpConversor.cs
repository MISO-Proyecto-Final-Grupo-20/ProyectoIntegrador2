namespace StoreFlow.Compras.API.Errores.Conversores;

public static class ErrorHttpConversor
{
    public static IResult Convertir(ErrorDeNegocio error)
    {
        return error.Categoria switch
        {
            "CONFLICTO_NEGOCIO" => Results.Conflict(new { mensaje = error.Mensaje }),
            _ => Results.BadRequest(new { mensaje = error.Mensaje })
        };
    }
}