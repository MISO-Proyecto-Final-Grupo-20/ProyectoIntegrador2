using StoreFlow.Compras.API.Comunes;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Entidades;
using StoreFlow.Compras.API.Errores.Fabricantes;

namespace StoreFlow.Compras.API.Servicios
{
    public class FabricantesService : IFabricantesService
    {
        private readonly ComprasDbContext _comprasDbContext;

        public FabricantesService(ComprasDbContext comprasDbContext)
        {
            _comprasDbContext = comprasDbContext;
        }

        public async Task<Resultado<CrearFabricanteResponse>> CrearFabricanteAsync(
            CrearFabricanteRequest crearFabricanteRequest)
        {
            if (ExisteUnFabricanteConElCorreo(crearFabricanteRequest.Correo))
            {
                var error = new FabricanteYaExiste(crearFabricanteRequest.Correo);
                return Resultado<CrearFabricanteResponse>.Falla(error);
            }


            var fabricante = new Fabricante()
            {
                RazonSocial = crearFabricanteRequest.Nombre,
                CorreoElectronico = crearFabricanteRequest.Correo
            };

            await _comprasDbContext.Fabricantes.AddAsync(fabricante);
            await _comprasDbContext.SaveChangesAsync();

            var fabricanteCreado =
                new CrearFabricanteResponse(fabricante.Id, fabricante.RazonSocial, fabricante.CorreoElectronico);

            return Resultado<CrearFabricanteResponse>.Exito(fabricanteCreado);
        }

        private bool ExisteUnFabricanteConElCorreo(string correoElectronico)
        {
            return _comprasDbContext.Fabricantes.Any(f => f.CorreoElectronico == correoElectronico);
        }
    }
}