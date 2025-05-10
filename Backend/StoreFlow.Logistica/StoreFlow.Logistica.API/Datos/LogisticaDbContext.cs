using Microsoft.EntityFrameworkCore;
using StoreFlow.Logistica.API.Entidades;

namespace StoreFlow.Logistica.API.Datos;

public class LogisticaDbContext(DbContextOptions<LogisticaDbContext> options) : DbContext(options)

{
    public DbSet<Entrega> Entregas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entrega>(entidad =>
        {
            entidad.ToTable("Entregas");
            entidad.HasKey(e => e.Id);

            entidad.HasMany(e => e.ProductosPedidos)
                .WithOne()
                .HasForeignKey("IdEntrega")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductoPedido>(entidad =>
        {
            entidad.ToTable("EntregasProductos");
            entidad.HasKey(e => new {e.IdEntrega, e.IdProducto});
            entidad.Property(e => e.IdProducto).ValueGeneratedNever();

            entidad.Property(e => e.Cantidad)
                .IsRequired();

            entidad.Property(e => e.Precio)
                .IsRequired();

            entidad.Property(e => e.Codigo).HasMaxLength(50);
            entidad.Property(e => e.Nombre).HasMaxLength(150);
        });
    }
}