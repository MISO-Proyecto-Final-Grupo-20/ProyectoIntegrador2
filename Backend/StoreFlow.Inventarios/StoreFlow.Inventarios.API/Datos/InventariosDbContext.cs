using Microsoft.EntityFrameworkCore;

namespace StoreFlow.Inventarios.API.Datos;

public class InventariosDbContext(DbContextOptions<InventariosDbContext> options) : DbContext(options)
{
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
    }
}