using Microsoft.EntityFrameworkCore;
using OpenAI;
using OpenAI.Chat;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.Entidades;

namespace StoreFlow.Ventas.API.Servicios;

public class ProcesadorDeVideo : IProcesadorDeVideo
{
    private readonly VentasDbContext _dbContext;
    private readonly IGeneradorDeRecomendacion _generador;
    private readonly ILogger<ProcesadorDeVideo> _logger;

    private static readonly string[] _prompts = new[]
    {
        "Una tienda de barrio tiene productos básicos mal organizados. ¿Qué sugerencias harías para mejorar la visibilidad y rotación de productos?",
        "Un pequeño supermercado tiene estanterías con productos mezclados sin una lógica clara. ¿Qué estrategias de reorganización recomendarías para facilitar la experiencia de compra?",
        "Un distribuidor minorista tiene parte del inventario en la sala de exhibición. ¿Qué sugerencias harías para optimizar el espacio y separar mejor exhibición y almacenamiento?",
        "En un negocio pequeño, los productos impulso no están cerca del punto de pago. ¿Qué ubicación y productos sugerirías para mejorar las ventas rápidas?",
        "Un tendero tiene mezclados productos de limpieza y alimentos. ¿Cómo podrías mejorar la organización por categorías?",
        "En un supermercado de barrio, hay mucho espacio vacío en las repisas superiores. ¿Qué productos se podrían ubicar ahí para aprovechar mejor el espacio?",
        "El negocio quiere introducir nuevos productos al surtido actual. ¿Qué tipos de productos complementarios sugerirías y dónde ubicarlos para captar atención?",
        "La tienda tiene pasillos angostos y clientes que se detienen en zonas mal organizadas. ¿Qué distribución del mobiliario ayudaría a mejorar la circulación?"
    };


    public ProcesadorDeVideo(
        VentasDbContext dbContext,
        IGeneradorDeRecomendacion generador,
        ILogger<ProcesadorDeVideo> logger)
    {
        _dbContext = dbContext;
        _generador = generador;
        _logger = logger;
    }

    public async Task ProcesarVideoAsync(int visitaId)
    {
        var visita = await _dbContext.Visitas
            .Include(v => v.Video)
            .FirstOrDefaultAsync(v => v.Id == visitaId);

        if (visita?.Video == null)
        {
            _logger.LogWarning("No se encontró la visita o el video para la visita {VisitaId}", visitaId);
            return;
        }

        var prompt = SeleccionarPromptAleatorio();
        _logger.LogInformation("Prompt utilizado para visita {VisitaId}: {Prompt}", visitaId, prompt);

        var textoRespuesta = await _generador.GenerarAsync(prompt);

        visita.Video.Recomendacion = textoRespuesta;
        visita.Video.Estado = EstadoProcesamiento.Procesado;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Visita {VisitaId} procesada correctamente", visitaId);
    }

    private static string SeleccionarPromptAleatorio()
    {
        var random = new Random();
        return _prompts[random.Next(_prompts.Length)];
    }
}