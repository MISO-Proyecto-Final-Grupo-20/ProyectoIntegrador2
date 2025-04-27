using MassTransit;
using Serilog;
using StoreFlow.Compartidos.Core.Infraestructura;
using StoreFlow.Orquestador.Worker.CreacionPedido;

var builder = Host.CreateApplicationBuilder(args);

const string serviceName = "Orquestador";
string openTelemetryEndpoint = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("OTEL_ENDPOINT");
string redisUrl = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("REDIS_URL");
string rabbitHost = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("RABBITMQ_HOST");

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", serviceName)
    .WriteTo.Console()
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = openTelemetryEndpoint;
        options.ResourceAttributes.Add("service.name", serviceName);
    })
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);


// Configurar MassTransit con RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<CreacionPedidoMachineState, CreacionPedidoState>().RedisRepository(redisUrl);
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitHost);
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.ConfigurarOpenTelemetry(serviceName, openTelemetryEndpoint);


var host = builder.Build();
host.Run();