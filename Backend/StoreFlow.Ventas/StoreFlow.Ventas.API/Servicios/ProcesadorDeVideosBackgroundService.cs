using Microsoft.EntityFrameworkCore;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.API.Servicios;

public class ProcesadorDeVideosBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProcesadorDeVideosBackgroundService> _logger;
    private readonly TimeSpan _intervalo = TimeSpan.FromSeconds(10);

    public ProcesadorDeVideosBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<ProcesadorDeVideosBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de procesamiento de videos iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<VentasDbContext>();
                var procesador = scope.ServiceProvider.GetRequiredService<IProcesadorDeVideo>();

                var videosPendientes = await dbContext.Visitas
                    .Include(v => v.Video)
                    .Where(v => v.Video != null && v.Video.Estado == EstadoProcesamiento.Pendiente)
                    .ToListAsync(stoppingToken);

                foreach (var visita in videosPendientes)
                {
                    _logger.LogInformation($"Procesando visita {visita.Id}...");
                    await procesador.ProcesarVideoAsync(visita.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar videos.");
            }

            await Task.Delay(_intervalo, stoppingToken);
        }

        _logger.LogInformation("Servicio de procesamiento de videos detenido.");
    }
}