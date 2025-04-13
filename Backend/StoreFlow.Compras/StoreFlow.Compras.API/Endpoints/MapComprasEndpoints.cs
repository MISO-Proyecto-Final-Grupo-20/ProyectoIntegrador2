using MiniValidation;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Errores.Conversores;
using StoreFlow.Compras.API.Servicios;

namespace StoreFlow.Compras.API.Endpoints;

public static class ComprasEndpoints
{
    public static void MapComprasEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/fabricantes", async (CrearFabricanteRequest crearFabricanteDto, IFabricantesService servicioFabricantes) =>
        {
            if (!MiniValidator.TryValidate(crearFabricanteDto, out var errors))
                return Results.ValidationProblem(errors);

            var resultado = await servicioFabricantes.CrearFabricanteAsync(crearFabricanteDto);

            if(!resultado.EsExitoso)
                return ErrorHttpConversor.Convertir(resultado.Error!);

            return Results.Created($"/fabricantes/{resultado.Valor!.Id}", resultado.Valor);
        }).RequireAuthorization("SoloUsuariosCcp");
    }
}