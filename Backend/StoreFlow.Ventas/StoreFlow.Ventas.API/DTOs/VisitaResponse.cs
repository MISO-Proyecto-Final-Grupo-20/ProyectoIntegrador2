namespace StoreFlow.Ventas.API.DTOs;

public record ArchivoResponse(string Nombre, long Tamanio, string Url);

public record VisitaResponse(
    int Id,
    DateTime Fecha,
    string Hora,
    ArchivoResponse Archivo
);