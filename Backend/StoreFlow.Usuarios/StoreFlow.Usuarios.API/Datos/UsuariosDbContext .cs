using Microsoft.EntityFrameworkCore;
using StoreFlow.Usuarios.API.Entidades;

namespace StoreFlow.Usuarios.API.Datos;

public class UsuariosDbContext(DbContextOptions<UsuariosDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
}