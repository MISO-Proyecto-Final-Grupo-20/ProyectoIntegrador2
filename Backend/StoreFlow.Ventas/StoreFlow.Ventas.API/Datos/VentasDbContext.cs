using Microsoft.EntityFrameworkCore;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.API.Datos;

public class VentasDbContext(DbContextOptions<VentasDbContext>options) : DbContext(options)
{
    public DbSet<Pedido> Pedidos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Pedido>(entidad =>
        {
            entidad.ToTable("Pedidos");
            entidad.HasKey(e => e.Id);
            
            entidad.HasMany(e => e.ProductosPedidos)
                .WithOne()
                .HasForeignKey("IdPedido")
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<ProductoPedido>(entidad =>
        {
            entidad.ToTable("ProductosPedidos");
            entidad.HasKey(e => new {e.IdPedido, e.IdProducto});
            entidad.Property(e => e.IdProducto).ValueGeneratedNever();
            
            entidad.Property(e => e.Cantidad)
                .IsRequired();
            
            entidad.Property(e => e.Precio)
                .IsRequired();

            entidad.Property(e => e.Codigo).HasMaxLength(50);
            entidad.Property(e => e.Nombre).HasMaxLength(150);
            
        });
    }

    public async Task GuardarPedidoAsync(Pedido pedido)
    {
        await Pedidos.AddAsync(pedido);
        await SaveChangesAsync();
    }
    
    public async Task<List<PedidoResponse>> ObtenerPedidosAsync(int idUsuario)
    {
        var pedidos = await Pedidos
            .Include(p => p.ProductosPedidos.Where(pp => pp.TieneInventario))
            .Where(p => p.IdCliente == idUsuario)
            .ToListAsync();
        
        return pedidos.Select(p => p.ConvertirAResponse()).ToList();
    }
}