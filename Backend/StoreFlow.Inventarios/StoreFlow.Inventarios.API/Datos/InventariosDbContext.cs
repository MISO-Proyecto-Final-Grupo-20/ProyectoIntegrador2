using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Inventarios.API.Entidades;

namespace StoreFlow.Inventarios.API.Datos;

public class InventariosDbContext(DbContextOptions<InventariosDbContext> options) : DbContext(options)
{
    public DbSet<Inventario> Inventarios { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Inventario>(entidad =>
        {
            entidad.ToTable("Inventarios");
            entidad.HasKey(e => e.IdProducto);

            entidad.Property(e => e.Cantidad)
                .IsRequired();
        });
    }
    
    public async Task<SolicitudDePedido> ValidarPedidoConInventarioAsync(SolicitudDePedido pedido)
    {
        var idSProductosSolicitados = pedido.productosSolicitados
            .Select(p => p.Id)
            .ToList();

        var inventarios = await Inventarios
            .Where(i => idSProductosSolicitados.Contains(i.IdProducto))
            .ToListAsync();
        
        var productosConEstadoInventario = pedido.productosSolicitados
                    .Select(productoSolicitado =>
                    {
                        var inventario = inventarios.FirstOrDefault(i => i.IdProducto == productoSolicitado.Id);
                        var tieneInventario = inventario != null && inventario.Cantidad >= productoSolicitado.Cantidad;
        
                        return productoSolicitado with {TieneInventario = tieneInventario};
                    })
                    .ToArray();
        
        return pedido with { productosSolicitados = productosConEstadoInventario };
    }
}