using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Inventarios.API.Entidades;

namespace StoreFlow.Inventarios.API.Datos;

public class InventariosDbContext(DbContextOptions<InventariosDbContext> options) : DbContext(options)
{
    public DbSet<Inventario> Inventarios { get; set; }
    public DbSet<Bodega> Bodegas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bodega>(b =>
        {
            b.ToTable("Bodegas");
            b.HasKey(x => x.Id);
            b.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Inventario>(i =>
        {
            i.ToTable("Inventarios");
            i.HasKey(x => new { x.IdProducto, x.IdBodega });

            i.Property(x => x.Cantidad).IsRequired();

            i.HasOne(x => x.Bodega)
                .WithMany(b => b.Inventarios)
                .HasForeignKey(x => x.IdBodega);
        });
    }

    public async Task<bool> ExisteInventarioSuficienteAsync(int idProducto, int cantidad)
    {
        var cantidadTotal = await Inventarios
            .Where(i => i.IdProducto == idProducto)
            .SumAsync(i => i.Cantidad);

        return cantidadTotal >= cantidad;
    }

    public async Task<SolicitudDePedido> ValidarPedidoConInventarioAsync(SolicitudDePedido pedido)
    {
        var idsProductos = pedido.ProductosSolicitados.Select(p => p.Id).ToList();

        var inventarios = await Inventarios
            .Where(i => idsProductos.Contains(i.IdProducto))
            .ToListAsync();

        var productosConEstadoInventario = pedido.ProductosSolicitados
            .Select(producto =>
            {
                var totalDisponible = inventarios
                    .Where(i => i.IdProducto == producto.Id)
                    .Sum(i => i.Cantidad);

                var tieneInventario = totalDisponible >= producto.Cantidad;
                return producto with { TieneInventario = tieneInventario };
            })
            .ToArray();

        var pedidoValidado = pedido with { ProductosSolicitados = productosConEstadoInventario };

        await DescontarInventarioAsync(pedidoValidado, inventarios);

        return pedidoValidado;
    }

    private async Task DescontarInventarioAsync(SolicitudDePedido pedido, List<Inventario> inventarios)
    {
        var productosConInventario = pedido.ProductosSolicitados
            .Where(p => p.TieneInventario)
            .ToList();

        foreach (var producto in productosConInventario)
        {
            var cantidadRestante = producto.Cantidad;

            var inventariosProducto = inventarios
                .Where(i => i.IdProducto == producto.Id && i.Cantidad > 0)
                .OrderByDescending(i => i.Cantidad)
                .ToList();

            foreach (var inventario in inventariosProducto)
            {
                if (cantidadRestante == 0) break;

                var descuento = Math.Min(cantidadRestante, inventario.Cantidad);
                inventario.Cantidad -= descuento;
                cantidadRestante -= descuento;

                Inventarios.Update(inventario);
            }
        }

        await SaveChangesAsync();
    }
}