using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Entidades;

namespace StoreFlow.Compras.API.Servicios
{
    public class FabricantesService : IFabricantesService
    {
        private readonly ComprasDbContext _comprasDbContext;

        public FabricantesService(ComprasDbContext comprasDbContext)
        {
            _comprasDbContext = comprasDbContext;
        }

        public async Task<CrearFabricanteResponse> CrearFabricanteAsync(CrearFabricanteRequest crearFabricanteRequest)
        {
            var fabricante = new Fabricante()
            {
                RazonSocial = crearFabricanteRequest.Nombre,
                CorreoElectronico = crearFabricanteRequest.CorreoElectronico
            };

            await _comprasDbContext.Fabricantes.AddAsync(fabricante);
            await _comprasDbContext.SaveChangesAsync();

            var fabricanteCreado =
                new CrearFabricanteResponse(fabricante.Id, fabricante.RazonSocial, fabricante.CorreoElectronico);

            return fabricanteCreado;
        }
    }
}
