namespace StoreFlow.Compras.API.DTOs;

public record RegistrarProducto(
    string Nombre,
    string Codigo,
    decimal Precio,
    string Imagen,
    int FabricanteAsociado);