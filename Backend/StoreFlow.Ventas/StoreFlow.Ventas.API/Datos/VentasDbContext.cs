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
        });
    }

    public async Task GuardarPedidoAsync(Pedido pedido)
    {
        await Pedidos.AddAsync(pedido);
        await SaveChangesAsync();
    }
}