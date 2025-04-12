using StoreFlow.Compras.API.Comunes;
using StoreFlow.Compras.API.DTOs;

namespace StoreFlow.Compras.API.Servicios;

public interface IFabricantesService
{
    Task<Resultado<CrearFabricanteResponse>> CrearFabricanteAsync(CrearFabricanteRequest crearFabricanteRequest);
}