namespace StoreFlow.Compras.API.DTOs;

public record ProductoResponse(
    string Imagen,
    string Nombre,
    string Codigo,
    decimal Precio);

public record ProductoCatalogoResponse(
    int Id,
    string Nombre,
    FabricanteResponse Fabricante,
    string Codigo,
    decimal Precio,
    string Imagen);

public record FabricanteResponse(int Id, string Nombre);