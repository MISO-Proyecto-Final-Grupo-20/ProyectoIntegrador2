using MiniValidation;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Errores.Conversores;
using StoreFlow.Compras.API.Servicios;

namespace StoreFlow.Compras.API.Endpoints;

public static class ComprasEndpoints
{
    public static void MapComprasEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/fabricantes",
            async (CrearFabricanteRequest crearFabricanteDto, IFabricantesService servicioFabricantes) =>
            {
                if (!MiniValidator.TryValidate(crearFabricanteDto, out var errors))
                    return Results.ValidationProblem(errors);

                var resultado = await servicioFabricantes.CrearFabricanteAsync(crearFabricanteDto);

                if (!resultado.EsExitoso)
                    return ErrorHttpConversor.Convertir(resultado.Error!);

                return Results.Created($"/fabricantes/{resultado.Valor!.Id}", resultado.Valor);
            }).RequireAuthorization("SoloUsuariosCcp");

        app.MapGet("/fabricantes", async (IFabricantesService servicioFabricantes) =>
        {
            var resultado = await servicioFabricantes.ObtenerListadoAsync();
            return Results.Ok(resultado);
        }).RequireAuthorization("SoloUsuariosCcp");


        app.MapPost("/productos", async (
            CrearProductoRequest request,
            IProductosService productosService) =>
        {
            if (!MiniValidator.TryValidate(request, out var errors))
                return Results.ValidationProblem(errors);

            var resultado = await productosService.CrearProductoAsync(request);

            if (!resultado.EsExitoso)
                return ErrorHttpConversor.Convertir(resultado.Error!);

            return Results.Created($"/productos/{resultado.Valor!.Id}", resultado.Valor);
        }).RequireAuthorization("SoloUsuariosCcp");

        app.MapPost("/productos/masivo", async (
            HttpRequest request,
            IProductosService productosService) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest("Debe enviar el archivo como multipart/form-data.");

            var form = await request.ReadFormAsync();
            var archivoCsv = form.Files.GetFile("archivo");

            if (archivoCsv is null || archivoCsv.Length == 0)
                return Results.BadRequest("El archivo CSV es obligatorio.");

            var resultado = await productosService.ValidarProductosMasivoAsync(archivoCsv);

            return Results.Ok(resultado);
        }).RequireAuthorization("SoloUsuariosCcp");


        app.MapPost("/productos/guardar-masivo", async (
            List<RegistrarProducto> productos,
            IProductosService service) =>
        {
            if (productos == null || productos.Count == 0)
                return Results.BadRequest("Debe enviar al menos un producto.");

            await service.GuardarProductosMasivosAsync(productos);
            return Results.NoContent();
        }).RequireAuthorization("SoloUsuariosCcp");


        app.MapGet("/productos", async (IProductosService productosService) =>
        {
            var resultado = await productosService.ObtenerProductosAsync();
            return Results.Ok(resultado);
        });
    }
}