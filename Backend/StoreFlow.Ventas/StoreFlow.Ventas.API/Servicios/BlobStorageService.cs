using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StoreFlow.Compartidos.Core.Infraestructura;
using System.Diagnostics.CodeAnalysis;

namespace StoreFlow.Ventas.API.Servicios;

[ExcludeFromCodeCoverage]
public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _contenedor;

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = EnvironmentUtilidades.ObtenerVariableEntornoRequerida("AZURE_STORAGE_CONNECTION_STRING");

        var containerName = "visitas";

        var blobServiceClient = new BlobServiceClient(connectionString);
        _contenedor = blobServiceClient.GetBlobContainerClient(containerName);

        _contenedor.CreateIfNotExists(PublicAccessType.None);
    }

    public async Task<string> SubirVideoAsync(IFormFile archivo, string nombreArchivo)
    {
        var blobClient = _contenedor.GetBlobClient(nombreArchivo);

        using var stream = archivo.OpenReadStream();
        await blobClient.UploadAsync(stream, true);

        return blobClient.Uri.ToString();
    }
}