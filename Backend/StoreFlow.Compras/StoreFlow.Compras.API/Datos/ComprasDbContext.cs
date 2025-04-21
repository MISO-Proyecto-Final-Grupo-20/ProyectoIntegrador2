using Microsoft.EntityFrameworkCore;
using StoreFlow.Compras.API.Entidades;

namespace StoreFlow.Compras.API.Datos;

public class ComprasDbContext(DbContextOptions<ComprasDbContext> options) : DbContext(options)
{
    public DbSet<Fabricante> Fabricantes => Set<Fabricante>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Fabricante>(fabricante =>
        {
            fabricante.HasIndex(f => f.CorreoElectronico).IsUnique();

            fabricante.HasMany(f => f.Productos)
                .WithOne(p => p.Fabricante)
                .HasForeignKey(p => p.FabricanteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Producto>(producto => { producto.HasIndex(p => p.Sku).IsUnique(); });
    }
}