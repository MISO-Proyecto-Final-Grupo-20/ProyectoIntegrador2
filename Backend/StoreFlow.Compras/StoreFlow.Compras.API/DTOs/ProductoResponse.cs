namespace StoreFlow.Compras.API.DTOs;

public record ProductoResponse(
    string Imagen,
    string Nombre,
    string Codigo,
    decimal Precio);