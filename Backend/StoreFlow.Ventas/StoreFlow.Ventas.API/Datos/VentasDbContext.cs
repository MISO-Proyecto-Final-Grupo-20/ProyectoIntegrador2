using Microsoft.EntityFrameworkCore;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.API.Datos;

public class VentasDbContext(DbContextOptions<VentasDbContext>options) : DbContext(options)
{
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PeriodoTiempo> PeriodosTiempo { get; set; }
    public DbSet<PlanVenta> PlanesVenta { get; set; }
    
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
        
        modelBuilder.Entity<PeriodoTiempo>(entidad =>
        {
            entidad.ToTable("PeriodosTiempo");
            entidad.HasKey(e => e.Id);
            
            entidad.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
                
            entidad.HasMany(e => e.PlanesVenta)
                .WithOne(p => p.PeriodoTiempo)
                .HasForeignKey(p => p.PeriodoTiempoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<PlanVenta>(entidad =>
        {
            entidad.ToTable("PlanesVenta");
            entidad.HasKey(e => e.Id);
            
            entidad.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
                
            entidad.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(500);
                
            entidad.Property(e => e.Precio)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        });
    }

    public async Task GuardarPedidoAsync(Pedido pedido)
    {
        await Pedidos.AddAsync(pedido);
        await SaveChangesAsync();
    }
}