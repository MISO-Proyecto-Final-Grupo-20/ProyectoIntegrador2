namespace StoreFlow.Compras.API.Errores.Productos;

public class SkuYaExiste(string sku) : ErrorDeNegocio
{
    public override string Categoria => "CONFLICTO_NEGOCIO";
    public override string Mensaje => $"Ya existe un producto con el código SKU {sku}";
}