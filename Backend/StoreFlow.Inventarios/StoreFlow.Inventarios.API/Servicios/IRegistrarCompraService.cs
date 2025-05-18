using StoreFlow.Inventarios.API.DTOs;

namespace StoreFlow.Inventarios.API.Servicios;

public interface IRegistrarCompraService
{
    Task RegistrarCompraAsync(RegistroCompraBodegaDto dto);
}