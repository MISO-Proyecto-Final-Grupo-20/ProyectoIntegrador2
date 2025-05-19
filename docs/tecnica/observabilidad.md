# 6. Observabilidad

StoreFlow implementa capacidades de observabilidad para rastrear, medir y diagnosticar el comportamiento de los microservicios que conforman el sistema. Esto incluye métricas, trazas distribuidas y logs estructurados.

---

## 6.1 Herramientas utilizadas

- **OpenTelemetry**: Recolección de métricas y trazas.
- **Serilog**: Generación de logs estructurados con exportación a OTLP.
- **Aspire Dashboard**: Visualización de trazas, métricas y eventos en tiempo real.
- **Azure Kubernetes Service (AKS)**: Monitorización básica mediante herramientas integradas de Azure.

---

## 6.2 Métricas recolectadas

- Solicitudes HTTP entrantes (duración, conteo, errores).
- Operaciones en Entity Framework Core.
- Métricas de cola en MassTransit.
- Conexiones y eventos de SignalR.
- Actividades de Redis (si aplica).
- Métricas personalizadas vía `AddMeter` en OpenTelemetry.

---

## 6.3 Trazabilidad distribuida

Los servicios generan trazas automáticamente usando los siguientes instrumentos:

- `AddAspNetCoreInstrumentation`
- `AddMassTransit`
- `AddEntityFrameworkCoreInstrumentation`
- `AddRedisInstrumentation`
- `AddSignalRInstrumentation`

Todas las trazas se exportan mediante el protocolo OTLP hacia el endpoint definido por la variable de entorno `OTEL_ENDPOINT`, el cual es consumido por **Aspire Dashboard** para su visualización.

---

## 6.4 Logging

Los logs se generan con **Serilog** y se enriquecen con información del contexto, como el nombre del servicio (`Application`) y propiedades de cada traza. Los logs se escriben en:

- Consola (útil para debugging local).
- Exportador OpenTelemetry, que permite que Aspire Dashboard los muestre junto con las trazas.

Configuración relevante:

```csharp
.WriteTo.Console()
.WriteTo.OpenTelemetry(options =>
{
    options.Endpoint = openTelemetryEndpoint;
    options.ResourceAttributes.Add("service.name", serviceName);
})
```

---

## 6.5 Visualización con Aspire Dashboard

Aspire Dashboard actúa como punto central de observabilidad. Permite visualizar:

- Trazas distribuidas entre servicios.
- Logs asociados a las trazas.
- Métricas básicas en tiempo real.

No se utiliza actualmente integración con herramientas como Grafana, Prometheus o Elastic APM.

---

## 6.6 Configuración

La configuración de observabilidad se realiza desde una clase común (`ConfiguracionesExtensiones.cs`) que centraliza:

- La inicialización de OpenTelemetry.
- La definición de `ResourceBuilder`.
- La exportación de métricas y trazas.
- La configuración de Serilog con salida a OTLP.

El endpoint de OpenTelemetry es configurable mediante la variable de entorno `OTEL_ENDPOINT`.

---

> ✅ Esta implementación permite a los desarrolladores tener visibilidad de lo que ocurre en los servicios y facilita el diagnóstico de errores y cuellos de botella durante el desarrollo y pruebas.


[⬅️ Volver al índice](index.md)