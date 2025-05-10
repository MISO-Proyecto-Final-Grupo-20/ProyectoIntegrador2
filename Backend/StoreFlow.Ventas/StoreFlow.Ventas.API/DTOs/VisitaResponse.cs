namespace StoreFlow.Ventas.API.DTOs;

public record VisitaResponse(
    int Id,
    DateTime Fecha,
    string Estado,
    string? Recomendacion,
    string UrlVideo
);