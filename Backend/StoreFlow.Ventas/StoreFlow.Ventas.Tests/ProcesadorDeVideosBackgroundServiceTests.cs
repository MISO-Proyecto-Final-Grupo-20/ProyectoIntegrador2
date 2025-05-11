using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.Entidades;
using StoreFlow.Ventas.API.Servicios;

namespace StoreFlow.Ventas.Tests;

public class ProcesadorDeVideosBackgroundServiceTests
{
    [Fact]
    public async Task DebeProcesarVisitasPendientes()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<VentasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new VentasDbContext(options);

        dbContext.Visitas.Add(new Visita
        {
            Id = 1,
            IdCliente = 100,
            IdVendedor = 200,
            Fecha = DateTime.UtcNow,
            Video = new Video
            {
                Estado = EstadoProcesamiento.Pendiente,
                Url = "https://fake"
            }
        });

        await dbContext.SaveChangesAsync();

        var procesador = Substitute.For<IProcesadorDeVideo>();
        var scopeServiceProvider = Substitute.For<IServiceProvider>();
        scopeServiceProvider.GetService(typeof(VentasDbContext)).Returns(dbContext);
        scopeServiceProvider.GetService(typeof(IProcesadorDeVideo)).Returns(procesador);

        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(scopeServiceProvider);

        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory.CreateScope().Returns(scope);

        var rootServiceProvider = Substitute.For<IServiceProvider>();
        rootServiceProvider.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);

        var logger = Substitute.For<ILogger<ProcesadorDeVideosBackgroundService>>();
        var servicio = new ProcesadorDeVideosBackgroundService(rootServiceProvider, logger);

        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(300)); // interrumpe rápido

        // Act
        await servicio.StartAsync(cts.Token);

        // Assert
        await procesador.Received(1).ProcesarVideoAsync(1);
    }

    [Fact]
    public async Task NoHaceNada_SiNoHayVisitasPendientes()
    {
        var options = new DbContextOptionsBuilder<VentasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var dbContext = new VentasDbContext(options);

        var procesador = Substitute.For<IProcesadorDeVideo>();
        var scopeServiceProvider = GetScopeProvider(dbContext, procesador);
        var scopeFactory = GetScopeFactory(scopeServiceProvider);
        var rootServiceProvider = Substitute.For<IServiceProvider>();
        rootServiceProvider.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);

        var logger = Substitute.For<ILogger<ProcesadorDeVideosBackgroundService>>();
        var servicio = new ProcesadorDeVideosBackgroundService(rootServiceProvider, logger);

        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(300));

        await servicio.StartAsync(cts.Token);

        await procesador.DidNotReceiveWithAnyArgs().ProcesarVideoAsync(default);
    }

    [Fact]
    public async Task CapturaErrores_AlProcesarVisitas()
    {
        var options = new DbContextOptionsBuilder<VentasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var dbContext = new VentasDbContext(options);

        dbContext.Visitas.Add(new Visita
        {
            Id = 1,
            IdCliente = 100,
            IdVendedor = 200,
            Fecha = DateTime.UtcNow,
            Video = new Video
            {
                Estado = EstadoProcesamiento.Pendiente,
                Url = "https://fake"
            }
        });

        await dbContext.SaveChangesAsync();

        var procesador = Substitute.For<IProcesadorDeVideo>();
        procesador.ProcesarVideoAsync(Arg.Any<int>()).Throws(new Exception("Simulación"));

        var scopeServiceProvider = GetScopeProvider(dbContext, procesador);
        var scopeFactory = GetScopeFactory(scopeServiceProvider);
        var rootServiceProvider = Substitute.For<IServiceProvider>();
        rootServiceProvider.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);

        var logger = Substitute.For<ILogger<ProcesadorDeVideosBackgroundService>>();
        var servicio = new ProcesadorDeVideosBackgroundService(rootServiceProvider, logger);

        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(300));

        await servicio.StartAsync(cts.Token);

        await procesador.Received(1).ProcesarVideoAsync(1);
        logger.Received().Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    private IServiceProvider GetScopeProvider(VentasDbContext dbContext, IProcesadorDeVideo procesador)
    {
        var scopeProvider = Substitute.For<IServiceProvider>();
        scopeProvider.GetService(typeof(VentasDbContext)).Returns(dbContext);
        scopeProvider.GetService(typeof(IProcesadorDeVideo)).Returns(procesador);
        return scopeProvider;
    }

    private IServiceScopeFactory GetScopeFactory(IServiceProvider scopeProvider)
    {
        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(scopeProvider);
        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory.CreateScope().Returns(scope);
        return scopeFactory;
    }
}