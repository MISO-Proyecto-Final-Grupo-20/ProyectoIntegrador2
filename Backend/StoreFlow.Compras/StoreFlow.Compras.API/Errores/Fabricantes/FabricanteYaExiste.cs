namespace StoreFlow.Compras.API.Errores.Fabricantes;

public class FabricanteYaExiste : ErrorDeNegocio
{
    private readonly string _correo;

    public FabricanteYaExiste(string correo)
    {
        _correo = correo;
    }

    public override string Categoria => "CONFLICTO_NEGOCIO";
    public override string Mensaje => $"Ya existe un fabricante con el correo {_correo}";
}