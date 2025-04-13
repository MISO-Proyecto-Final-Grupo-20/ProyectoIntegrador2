using Microsoft.EntityFrameworkCore;
using StoreFlow.Compras.API.Entidades;

namespace StoreFlow.Compras.API.Datos;

public class ComprasDbContext(DbContextOptions<ComprasDbContext> options) : DbContext(options)
{
    public DbSet<Fabricante> Fabricantes => Set<Fabricante>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Fabricante>()
            .HasIndex(f => f.CorreoElectronico)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}