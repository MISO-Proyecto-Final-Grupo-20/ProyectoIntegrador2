﻿using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;
using StoreFlow.Ventas.API.DTOs;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.API.Datos;

public class VentasDbContext(DbContextOptions<VentasDbContext> options) : DbContext(options)
{
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PlanDeVentas> PlanesDeVentas { get; set; }
    public DbSet<Visita> Visitas { get; set; }
    public DbSet<Video> Videos { get; set; }

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
            entidad.HasKey(e => new { e.IdPedido, e.IdProducto });
            entidad.Property(e => e.IdProducto).ValueGeneratedNever();

            entidad.Property(e => e.Cantidad)
                .IsRequired();

            entidad.Property(e => e.Precio)
                .IsRequired();

            entidad.Property(e => e.Codigo).HasMaxLength(50);
            entidad.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<PlanDeVentas>(entidad =>
        {
            entidad.ToTable("PlanesDeVentas");
            entidad.HasKey(e => new { e.PeriodoTiempo, e.IdVendedor });
            entidad.Property(p => p.NombreVendedor).HasMaxLength(200);
        });

        modelBuilder.Entity<Visita>(entidad =>
        {
            entidad.ToTable("Visitas");
            entidad.HasKey(e => e.Id);

            entidad.Property(e => e.Fecha)
                .IsRequired();

            entidad.HasOne(e => e.Video)
                .WithOne(v => v.Visita)
                .HasForeignKey<Video>(v => v.VisitaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Video>(entidad =>
        {
            entidad.ToTable("Videos");
            entidad.HasKey(e => e.Id);

            entidad.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(500);

            entidad.Property(e => e.Recomendacion)
                .HasMaxLength(1000);

            entidad.Property(e => e.Estado)
                .HasConversion<int>()
                .IsRequired();
        });
    }

    public async Task GuardarPedidoAsync(Pedido pedido)
    {
        await Pedidos.AddAsync(pedido);
        await SaveChangesAsync();
    }

    public async Task<List<PedidoResponse>> ObtenerPedidosAsync(int idCliente)
    {
        var pedidos = await Pedidos
            .Include(p => p.ProductosPedidos.Where(pp => pp.TieneInventario))
            .Where(p => p.IdCliente == idCliente)
            .ToListAsync();

        return pedidos.Select(p => p.ConvertirAResponse()).ToList();
    }

    public async Task<List<PedidoResponse>> ObtenerPedidosAsync(int idCliente, int idVendedor)
    {
        var pedidos = await Pedidos
            .Include(p => p.ProductosPedidos.Where(pp => pp.TieneInventario))
            .Where(p => p.IdVendedor == idVendedor && p.IdCliente == idCliente)
            .ToListAsync();

        return pedidos.Select(p => p.ConvertirAResponse()).ToList();
    }

    public async Task GuardarPlanesDeVentas(PlanDeVentas[] planesDeVentas)
    {
        var planesDeVentasActuales = await PlanesDeVentas.ToListAsync();

        var repetidos = (from actual in planesDeVentasActuales
                join nuevo in planesDeVentas on new { actual.PeriodoTiempo, actual.IdVendedor } equals new
                    { nuevo.PeriodoTiempo, nuevo.IdVendedor }
                select actual)
            .ToList();

        if (repetidos.Count > 0)
        {
            PlanesDeVentas.RemoveRange(repetidos);
            await SaveChangesAsync();
        }

        await PlanesDeVentas.AddRangeAsync(planesDeVentas);
        await SaveChangesAsync();
    }

    public async Task<ReporteVentasResponse[]> ObtenerReporteVentasAsync(ReporteVentasRequest request)
    {
        var fechaFinalParaReporte = DateTime.MinValue;
        if (request.FechaFinal.HasValue)
            fechaFinalParaReporte = request.FechaFinal.Value.AddDays(1);

        var pedidosObtenidos = await Pedidos
            .Include(p => p.ProductosPedidos)
            .Where(p =>
                (!request.Vendedor.HasValue || p.IdVendedor == request.Vendedor) &&
                (!request.FechaInicial.HasValue || p.FechaCreacion >= request.FechaInicial) &&
                (!request.FechaFinal.HasValue || p.FechaCreacion < fechaFinalParaReporte)
                && (!request.Producto.HasValue || p.ProductosPedidos.Any(pp => pp.IdProducto == request.Producto))
            )
            .SelectMany(p => p.ProductosPedidos, (pedido, producto) => new
            {
                Vendedor = pedido.NombreVendedor ?? "Venta directa",
                Fecha = pedido.FechaCreacion,
                Producto = producto.Nombre ?? "Producto sin nombre",
                Cantidad = producto.Cantidad,
                IdProducto = producto.IdProducto
            }).ToListAsync();

        return pedidosObtenidos
            .Where(p => !request.Producto.HasValue || p.IdProducto == request.Producto)
            .Select(p => new ReporteVentasResponse(p.Vendedor,
                p.Fecha, p.Producto, p.Cantidad)).ToArray();
    }
}