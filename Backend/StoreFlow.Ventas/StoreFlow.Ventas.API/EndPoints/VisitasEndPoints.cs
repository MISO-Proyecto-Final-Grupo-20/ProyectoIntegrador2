using Microsoft.EntityFrameworkCore;
using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.DTOs;
using StoreFlow.Ventas.API.Entidades;
using StoreFlow.Ventas.API.Servicios;

namespace StoreFlow.Ventas.API.EndPoints;

public static class VisitasEndPoints
{
    public static void MapVisitasEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/visitas", async (
            HttpRequest request,
            VentasDbContext dbContext,
            IBlobStorageService blobStorageService) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest("Debe enviar el video como multipart/form-data.");

            var form = await request.ReadFormAsync();
            var archivoVideo = form.Files.FirstOrDefault();

            if (archivoVideo is null || archivoVideo.Length == 0)
                return Results.BadRequest("El video es obligatorio.");

            if (!form.TryGetValue("idVendedor", out var idVendedorStr) ||
                !form.TryGetValue("idCliente", out var idClienteStr) ||
                !int.TryParse(idVendedorStr, out var idVendedor) ||
                !int.TryParse(idClienteStr, out var idCliente))
                return Results.BadRequest("Debe incluir los campos idVendedor e idCliente.");

            var nombreArchivo = $"visita_{Guid.NewGuid()}.mp4";
            var url = await blobStorageService.SubirVideoAsync(archivoVideo, nombreArchivo);

            var visita = new Visita
            {
                IdVendedor = idVendedor,
                IdCliente = idCliente,
                Fecha = DateTime.UtcNow,
                Video = new Video
                {
                    Url = url,
                    Estado = EstadoProcesamiento.Pendiente
                }
            };

            dbContext.Visitas.Add(visita);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new
            {
                visita.Id,
                visita.Fecha,
                UrlVideo = url,
                Estado = visita.Video.Estado.ToString(),
                mensaje = "Visita registrada con éxito."
            });
        }).RequireAuthorization("Vendedor");


        app.MapGet("/visitas/{clienteId:int}", async (
            int clienteId,
            VentasDbContext dbContext,
            HttpContext httpContext) =>
        {
            var vendedorId = UtilidadesEndPoints.RecuperarIdUsuarioToken(httpContext);
            var visitas = await dbContext.Visitas
                .Include(v => v.Video)
                .Where(v => v.IdVendedor == vendedorId && v.IdCliente == clienteId)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            var respuesta = visitas.Select(v => new VisitaResponse(
                v.Id,
                v.Fecha,
                v.Video?.Estado.ToString() ?? "SinVideo",
                v.Video?.Recomendacion ?? string.Empty,
                v.Video?.Url ?? string.Empty
            )).ToList();

            return Results.Ok(respuesta);
            return Results.Ok(respuesta);
        }).RequireAuthorization("Vendedor");
    }
}