using Microsoft.EntityFrameworkCore;

namespace StoreFlow.Ventas.API.Datos;

public class VentasDbContext(DbContextOptions<VentasDbContext>options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         base.OnModelCreating(modelBuilder);
    }
}