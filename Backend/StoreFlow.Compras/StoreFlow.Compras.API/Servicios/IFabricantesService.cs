using StoreFlow.Compras.API.DTOs;

namespace StoreFlow.Compras.API.Servicios;

public interface IFabricantesService
{
    Task<CrearFabricanteResponse> CrearFabricanteAsync(CrearFabricanteRequest crearFabricanteRequest);
}