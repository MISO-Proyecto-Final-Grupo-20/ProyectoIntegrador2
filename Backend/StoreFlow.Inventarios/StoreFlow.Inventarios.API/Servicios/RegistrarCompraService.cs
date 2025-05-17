using Microsoft.EntityFrameworkCore;
using StoreFlow.Inventarios.API.Datos;
using StoreFlow.Inventarios.API.DTOs;
using StoreFlow.Inventarios.API.Entidades;

namespace StoreFlow.Inventarios.API.Servicios;

public class RegistrarCompraService : IRegistrarCompraService
{
    private readonly InventariosDbContext _db;

    public RegistrarCompraService(InventariosDbContext db)
    {
        _db = db;
    }

    public async Task RegistrarCompraAsync(RegistroCompraBodegaDto dto)
    {
        // Validar que la bodega exista
        var bodegaExiste = await _db.Bodegas.AnyAsync(b => b.Id == dto.Bodega);
        if (!bodegaExiste)
            throw new InvalidOperationException($"La bodega con ID {dto.Bodega} no existe.");

        // Procesar productos
        foreach (var producto in dto.Productos)
        {
            var inventario = await _db.Inventarios
                .FirstOrDefaultAsync(i => i.IdProducto == producto.Id && i.IdBodega == dto.Bodega);

            if (inventario is null)
            {
                // Crear nuevo inventario
                inventario = new Inventario(producto.Id, dto.Bodega, producto.Cantidad);
                _db.Inventarios.Add(inventario);
            }
            else
            {
                // Sumar cantidad
                inventario.Cantidad += producto.Cantidad;
                _db.Inventarios.Update(inventario);
            }
        }

        await _db.SaveChangesAsync();
    }
}