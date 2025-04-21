using Microsoft.EntityFrameworkCore;
using StoreFlow.Compras.API.Comunes;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Entidades;
using StoreFlow.Compras.API.Errores.Productos;

namespace StoreFlow.Compras.API.Servicios;

public class ProductosService(ComprasDbContext db) : IProductosService
{
    public async Task<Resultado<CrearProductoResponse>> CrearProductoAsync(CrearProductoRequest request)
    {
        var existeSku = await db.Productos.AnyAsync(p => p.Sku == request.Codigo);
        if (existeSku)
            return Resultado<CrearProductoResponse>.Falla(new SkuYaExiste(request.Codigo));

        var producto = new Producto
        {
            Nombre = request.Nombre,
            Sku = request.Codigo,
            Precio = request.Precio,
            ImagenUrl = request.Imagen,
            FabricanteId = request.FabricanteAsociado
        };

        db.Productos.Add(producto);
        await db.SaveChangesAsync();

        var response = new CrearProductoResponse(producto.Id, producto.Nombre, producto.Sku);
        return Resultado<CrearProductoResponse>.Exito(response);
    }
}