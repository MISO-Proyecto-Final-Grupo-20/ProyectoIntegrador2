using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.Entidades;
using StoreFlow.Ventas.API.Servicios;

namespace StoreFlow.Ventas.Tests;

public class ProcesadorDeVideoTests
{
    [Fact]
    public async Task DebeProcesarVideo_ActualizarEstadoYRecomendacion()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<VentasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new VentasDbContext(options);

        var visita = new Visita
        {
            Id = 1,
            IdCliente = 101,
            IdVendedor = 202,
            Fecha = DateTime.UtcNow,
            Video = new Video
            {
                Url = "https://fake.blob/visita.mp4",
                Estado = EstadoProcesamiento.Pendiente
            }
        };

        dbContext.Visitas.Add(visita);
        await dbContext.SaveChangesAsync();

        var generador = Substitute.For<IGeneradorDeRecomendacion>();
        generador.GenerarAsync(Arg.Any<string>())
            .Returns("Organiza por categorías, ubica snacks en caja, resalta promociones.");

        var logger = Substitute.For<ILogger<ProcesadorDeVideo>>();
        var servicio = new ProcesadorDeVideo(dbContext, generador, logger);

        // Act
        await servicio.ProcesarVideoAsync(visita.Id);

        // Assert
        var resultado = await dbContext.Visitas.Include(v => v.Video).FirstAsync();
        Assert.Equal(EstadoProcesamiento.Procesado, resultado.Video.Estado);
        Assert.Contains("snacks", resultado.Video.Recomendacion);
    }

    [Fact]
    public async Task DebeIgnorar_VisitaNoExiste()
    {
        var options = new DbContextOptionsBuilder<VentasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new VentasDbContext(options);
        var generador = Substitute.For<IGeneradorDeRecomendacion>();
        var logger = Substitute.For<ILogger<ProcesadorDeVideo>>();

        var servicio = new ProcesadorDeVideo(dbContext, generador, logger);

        // Act
        await servicio.ProcesarVideoAsync(99); // ID que no existe

        // Assert
        await generador.DidNotReceive().GenerarAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task DebeIgnorar_VisitaSinVideo()
    {
        var options = new DbContextOptionsBuilder<VentasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new VentasDbContext(options);

        var visita = new Visita
        {
            Id = 1,
            IdCliente = 101,
            IdVendedor = 202,
            Fecha = DateTime.UtcNow,
            Video = null!
        };

        dbContext.Visitas.Add(visita);
        await dbContext.SaveChangesAsync();

        var generador = Substitute.For<IGeneradorDeRecomendacion>();
        var logger = Substitute.For<ILogger<ProcesadorDeVideo>>();

        var servicio = new ProcesadorDeVideo(dbContext, generador, logger);

        // Act
        await servicio.ProcesarVideoAsync(visita.Id);

        // Assert
        await generador.DidNotReceive().GenerarAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task DebeRegistrarError_SiGeneradorFalla()
    {
        var options = new DbContextOptionsBuilder<VentasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new VentasDbContext(options);

        var visita = new Visita
        {
            Id = 1,
            IdCliente = 101,
            IdVendedor = 202,
            Fecha = DateTime.UtcNow,
            Video = new Video
            {
                Url = "https://fake.blob/visita.mp4",
                Estado = EstadoProcesamiento.Pendiente
            }
        };

        dbContext.Visitas.Add(visita);
        await dbContext.SaveChangesAsync();

        var generador = Substitute.For<IGeneradorDeRecomendacion>();
        generador.GenerarAsync(Arg.Any<string>())
            .Throws(new InvalidOperationException("Simulación de error"));

        var logger = Substitute.For<ILogger<ProcesadorDeVideo>>();
        var servicio = new ProcesadorDeVideo(dbContext, generador, logger);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            servicio.ProcesarVideoAsync(visita.Id));

        Assert.Equal("Simulación de error", ex.Message);
    }
}