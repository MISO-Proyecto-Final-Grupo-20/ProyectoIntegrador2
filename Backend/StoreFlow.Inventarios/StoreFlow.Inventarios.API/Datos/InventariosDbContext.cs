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
            entidad.Property(e => e.IdProducto).ValueGeneratedNever();

            entidad.Property(e => e.Cantidad)
                .IsRequired();
        });
    }
    
    public async Task<bool> ExisteInventarioSuficienteAsync(int idProducto, int cantidad)
    {
        var inventario = await Inventarios
            .FirstOrDefaultAsync(i => i.IdProducto == idProducto);

        return inventario != null && inventario.Cantidad >= cantidad;
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

        var pedidoValidado = pedido with { productosSolicitados = productosConEstadoInventario };

        await DescontarInventarioAsync(pedidoValidado);

        return pedidoValidado;
    }

    private async Task DescontarInventarioAsync(SolicitudDePedido pedido)
    {
        var productosConInventario = pedido.productosSolicitados
            .Where(p => p.TieneInventario)
            .ToList();
        
        var inventarios = await Inventarios
            .Where(i => productosConInventario.Select(p => p.Id).Contains(i.IdProducto))
            .ToListAsync();

        (from inventario in inventarios
            join productoSolicitado in productosConInventario
                on inventario.IdProducto equals productoSolicitado.Id
            select new {inventarios = inventario, productoSolicitado})
            .ToList()
            .ForEach(x =>
            {
                x.inventarios.Cantidad -= x.productoSolicitado.Cantidad;
                Inventarios.Update(x.inventarios);
            });
        
        await SaveChangesAsync();
    }
}