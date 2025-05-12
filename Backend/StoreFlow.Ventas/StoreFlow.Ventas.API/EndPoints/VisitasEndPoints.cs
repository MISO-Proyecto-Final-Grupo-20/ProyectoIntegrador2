using System.Globalization;
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
        app.MapPost("/visitas/{clienteId:int}", async (
            int clienteId,
            HttpRequest request,
            VentasDbContext dbContext,
            IBlobStorageService blobStorageService) =>
        {
            if (!request.HasFormContentType)
                return Results.BadRequest("Debe enviar el video como multipart/form-data.");

            var form = await request.ReadFormAsync();
            var fechaStr = form["fecha"];
            var horaStr = form["hora"];

            if (string.IsNullOrWhiteSpace(fechaStr))
                return Results.BadRequest("Faltan campos requerido: fecha");
            if (string.IsNullOrWhiteSpace(fechaStr) || string.IsNullOrWhiteSpace(horaStr))
                return Results.BadRequest("Faltan campos requerido:hora");

            var fechaLimpia =
                fechaStr.ToString().Split("00:00:00")[0].Trim(); // elimina zona horaria y texto descriptivo
            var hora = horaStr.ToString().Trim();

            Console.WriteLine($"{fechaLimpia} {hora}");

            if (!DateTime.TryParse($"{fechaLimpia} {hora}", out var fechaHora))
                return Results.BadRequest("Fecha u hora inválida.");


            Console.WriteLine("fecha reconstruida:{0}", fechaHora);

            var fechaHoraUtc = new DateTime(
                fechaHora.Year,
                fechaHora.Month,
                fechaHora.Day,
                fechaHora.Hour,
                fechaHora.Minute,
                fechaHora.Second,
                DateTimeKind.Utc);
            var archivoVideo = form.Files.FirstOrDefault();

            if (archivoVideo is null || archivoVideo.Length == 0)
                return Results.BadRequest("El video es obligatorio.");


            var vendedorId = UtilidadesEndPoints.RecuperarIdUsuarioToken(request.HttpContext);

            var nombreArchivo = $"visita_{Guid.NewGuid()}.mp4";
            var url = await blobStorageService.SubirVideoAsync(archivoVideo, nombreArchivo);

            var visita = new Visita
            {
                IdVendedor = vendedorId,
                IdCliente = clienteId,
                Fecha = fechaHoraUtc,
                Video = new Video
                {
                    Url = url,
                    Estado = EstadoProcesamiento.Pendiente,
                    NombreOriginal = archivoVideo.FileName,
                    TamanioBytes = archivoVideo.Length
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
                v.Fecha.ToString("hh:mm"),
                new ArchivoResponse(
                    string.IsNullOrEmpty(v.Video?.NombreOriginal) ? "" : v.Video?.NombreOriginal,
                    v.Video.TamanioBytes,
                    v.Video.Url)
            )).ToList();
            return Results.Ok(respuesta);
        }).RequireAuthorization("Vendedor");

        app.MapGet("/visitas/analisis", async (
            VentasDbContext dbContext,
            HttpContext httpContext) =>
        {
            var visitas = await dbContext.Visitas
                .Include(v => v.Video)
                .Where(v =>
                    v.Video != null &&
                    v.Video.Estado == EstadoProcesamiento.Procesado)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            var respuesta = visitas.Select(v => new AnalisisVisitaResponse(
                v.Id,
                v.IdCliente.ToString(),
                v.Fecha,
                v.Fecha.ToString("hh:mm"),
                new ArchivoResponse(
                    v.Video!.NombreOriginal,
                    v.Video.TamanioBytes,
                    v.Video.Url
                ),
                v.Video.Recomendacion
            )).ToList();

            return Results.Ok(respuesta);
        }).RequireAuthorization("SoloUsuariosCcp");

        app.MapPost("/visitas/analisis/{idVisita:int}/observaciones", async (
            int idVisita,
            HttpRequest request,
            VentasDbContext dbContext,
            HttpContext httpContext) =>
        {
            using var reader = new StreamReader(request.Body);
            var nuevaObservacion = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(nuevaObservacion))
                return Results.BadRequest("La recomendación no puede estar vacía.");

            var visita = await dbContext.Visitas
                .Include(v => v.Video)
                .FirstOrDefaultAsync(v => v.Id == idVisita);

            if (visita is null)
                return Results.NotFound("No se encontró la visita.");

            if (visita.Video is null)
                return Results.BadRequest("La visita no tiene un video asociado.");

            visita.Video.Recomendacion = nuevaObservacion;
            await dbContext.SaveChangesAsync();

            return Results.Ok(new
            {
                mensaje = "Recomendación actualizada con éxito.",
                visita.Id,
                nuevaObservacion
            });
        }).RequireAuthorization("SoloUsuariosCcp");
    }
}