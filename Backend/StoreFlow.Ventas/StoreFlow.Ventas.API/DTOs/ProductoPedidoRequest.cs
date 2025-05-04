using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Ventas.API.DTOs;

public record ProductoPedidoRequest(string Codigo, int Cantidad, decimal Precio)
{
    public ProductoSolicitado CrearProductoPedido()
    {
        if(int.TryParse(Codigo, out int codigo) == false)
        {
            throw new ArgumentException("El código del producto no es válido.");
        }
        return new ProductoSolicitado(codigo, Cantidad, Precio, false);
    }
};