using AspNetCore.SignalR.OpenTelemetry;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace StoreFlow.Compartidos.Core.Infraestructura;

public static class ConfiguracionesExtensiones
{
    public static void ConfigurarAutenticacion(this IServiceCollection services)
    {
        var jwtSecret = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("JWT_SECRET");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = ClaimTypes.Role,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };
            });
    }

    public static void ConfigurarMasstransitRabbitMq(this IServiceCollection services, Assembly assembly)
    {
        string rabbitHost = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("RABBITMQ_HOST");
        services.AddMassTransit(x =>
        {
            x.AddConsumers(assembly);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitHost);
                cfg.ConfigureEndpoints(context);
                cfg.UseInMemoryOutbox(context);
            });
        });
    }

    public static void ConfigurarObservabilidad(this IHostBuilder builder, string serviceName)
    {
        string openTelemetryEndpoint = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("OTEL_ENDPOINT");
        builder.UsarSerilog(serviceName, openTelemetryEndpoint);
        builder.ConfigureServices(services =>
        {
            services.ConfigurarOpenTelemetry(serviceName, openTelemetryEndpoint);
        });
    }

    private static void UsarSerilog(this IHostBuilder builder, string serviceName, string openTelemetryEndpoint)
    {
        builder.UseSerilog((context, configuration) =>
        {
            configuration
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = openTelemetryEndpoint;
                    options.ResourceAttributes.Add("service.name", serviceName);
                })
                .Enrich.WithProperty("Application", serviceName);
        });
    }

    public static void ConfigurarOpenTelemetry(this IServiceCollection services, string serviceName,
        string openTelemetryEndpoint)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName)
            .AddTelemetrySdk()
            .AddContainerDetector();

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource("MassTransit")
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddRedisInstrumentation()
                    .AddSignalRInstrumentation()
                    .AddOtlpExporter(options => { options.Endpoint = new Uri(openTelemetryEndpoint); }
                    );
            })
            .WithMetrics(metricProviderBuilder =>
            {
                metricProviderBuilder
                    .SetResourceBuilder(resourceBuilder)
                    .AddMeter("MassTransit")
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(options => { options.Endpoint = new Uri(openTelemetryEndpoint); });
            });
    }
}