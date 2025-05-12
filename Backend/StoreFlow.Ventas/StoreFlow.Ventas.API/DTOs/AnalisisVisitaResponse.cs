namespace StoreFlow.Ventas.API.DTOs;

public record AnalisisVisitaResponse(
    int Id,
    string Cliente,
    DateTime Fecha,
    string Hora,
    ArchivoResponse Archivo,
    string Observaciones
);