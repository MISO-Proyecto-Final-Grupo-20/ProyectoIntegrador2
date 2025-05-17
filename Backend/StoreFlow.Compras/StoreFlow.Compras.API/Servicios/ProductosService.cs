using Microsoft.EntityFrameworkCore;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;
using StoreFlow.Compras.API.Comunes;
using StoreFlow.Compras.API.Datos;
using StoreFlow.Compras.API.DTOs;
using StoreFlow.Compras.API.Entidades;
using StoreFlow.Compras.API.Errores.Productos;

namespace StoreFlow.Compras.API.Servicios;

public class ProductosService(ComprasDbContext db) : IProductosService
{
    public async Task<Resultado<CrearProductoResponse>> CrearProductoAsync(CrearProductoRequest request)
    {
        var existeSku = await db.Productos.AnyAsync(p => p.Sku == request.Codigo);
        if (existeSku)
            return Resultado<CrearProductoResponse>.Falla(new SkuYaExiste(request.Codigo));

        var producto = new Producto
        {
            Nombre = request.Nombre,
            Sku = request.Codigo,
            Precio = request.Precio,
            ImagenUrl = request.Imagen,
            FabricanteId = request.FabricanteAsociado
        };

        db.Productos.Add(producto);
        await db.SaveChangesAsync();

        var response = new CrearProductoResponse(producto.Id, producto.Nombre, producto.Sku);
        return Resultado<CrearProductoResponse>.Exito(response);
    }

    public async Task<ProductoResponse[]> ObtenerProductosAsync()
    {
        var productos = await db.Productos
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return productos
            .Select(p => new ProductoResponse(
                p.ImagenUrl,
                p.Nombre,
                p.Id.ToString(),
                p.Precio,
                p.Id))
            .ToArray();
    }

    public async Task<List<InformacionPoducto>> ObtenerProductosAsync(int[] idsProductos)
    {
        var productos = await db.Productos
            .Where(p => idsProductos.Contains(p.Id))
            .ToListAsync();

        return productos
            .Select(p => new InformacionPoducto(p.Id,
                p.ImagenUrl,
                p.Nombre,
                p.Sku,
                p.Precio))
            .ToList();
    }

    public async Task<ResultadoCargaMasivaResponse> ValidarProductosMasivoAsync(IFormFile archivoCsv)
    {
        var lineas = await LeerLineasValidasAsync(archivoCsv);
        var datosCsv = ParsearLineas(lineas);

        var fabricantes = await ObtenerFabricantesAsync(datosCsv);
        var skusExistentes = await ObtenerSkusExistentesAsync(datosCsv);

        var skusDuplicadosEnArchivo = datosCsv
            .Where(x => x.Campos.Length >= 2)
            .Select(x => x.Campos[1].Trim())
            .GroupBy(sku => sku)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();

        var (_, productosDto, errores) = ValidarYConstruirProductos(
            datosCsv, fabricantes, skusExistentes, skusDuplicadosEnArchivo);

        return new ResultadoCargaMasivaResponse
        {
            Productos = productosDto,
            Errores = errores
        };
    }

    private async Task<List<Producto>> ValidarAntesDeGuardarAsync(List<RegistrarProducto> productos)
    {
        var datosCsv = productos
            .Select((p, index) => (Linea: index + 1,
                Campos: new[] { p.Nombre, p.Codigo, p.FabricanteAsociado.ToString(), p.Precio.ToString(), p.Imagen }))
            .ToList();

        var fabricantes = await ObtenerFabricantesAsync(datosCsv);
        var skusExistentes = await ObtenerSkusExistentesAsync(datosCsv);
        var skusDuplicadosEnArchivo = productos
            .GroupBy(p => p.Codigo)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();

        var (productosValidados, _, errores) = ValidarYConstruirProductos(
            datosCsv, fabricantes, skusExistentes, skusDuplicadosEnArchivo);

        if (errores.Any())
            throw new InvalidOperationException($"Errores en la carga masiva:\n{string.Join("\\n", errores)}");

        return productosValidados;
    }


    public async Task GuardarProductosMasivosAsync(List<RegistrarProducto> productos)
    {
        var skus = productos.Select(p => p.Codigo).ToList();

        var skusExistentes = await db.Productos
            .Where(p => skus.Contains(p.Sku))
            .Select(p => p.Sku)
            .ToListAsync();

        var nuevosProductos = productos
            .Where(p => !skusExistentes.Contains(p.Codigo))
            .Select(p => new Producto
            {
                Nombre = p.Nombre,
                Sku = p.Codigo,
                Precio = p.Precio,
                ImagenUrl = p.Imagen,
                FabricanteId = p.FabricanteAsociado
            })
            .ToList();

        if (nuevosProductos.Any())
        {
            db.Productos.AddRange(nuevosProductos);
            await db.SaveChangesAsync();
        }
    }


    private async Task<List<string>> LeerLineasValidasAsync(IFormFile archivo)
    {
        var lineas = new List<string>();
        using var reader = new StreamReader(archivo.OpenReadStream());
        while (!reader.EndOfStream)
        {
            var linea = await reader.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(linea))
                lineas.Add(linea.Trim());
        }

        return lineas;
    }

    private List<(int Linea, string[] Campos)> ParsearLineas(List<string> lineas)
    {
        return lineas
            .Select((linea, index) => (Linea: index + 1, Campos: linea.Split(',')))
            .ToList();
    }


    private async Task<Dictionary<int, Fabricante>> ObtenerFabricantesAsync(List<(int Linea, string[] Campos)> datos)
    {
        var ids = datos
            .Where(x => x.Campos.Length >= 3 && int.TryParse(x.Campos[2], out _))
            .Select(x => int.Parse(x.Campos[2]))
            .Distinct()
            .ToList();

        return await db.Fabricantes
            .Where(f => ids.Contains(f.Id))
            .ToDictionaryAsync(f => f.Id);
    }

    private async Task<List<string>> ObtenerSkusExistentesAsync(List<(int Linea, string[] Campos)> datos)
    {
        var skus = datos
            .Where(x => x.Campos.Length >= 2)
            .Select(x => x.Campos[1].Trim())
            .ToList();

        return await db.Productos
            .Where(p => skus.Contains(p.Sku))
            .Select(p => p.Sku)
            .ToListAsync();
    }

    private (List<Producto> Productos, List<ProductoCargadoDto> Dtos, List<string> Errores)
        ValidarYConstruirProductos(
            List<(int Linea, string[] Campos)> datos,
            Dictionary<int, Fabricante> fabricantes,
            List<string> skusExistentes,
            HashSet<string> skusDuplicadosEnArchivo)
    {
        var productos = new List<Producto>();
        var productosDto = new List<ProductoCargadoDto>();
        var errores = new List<string>();

        foreach (var (linea, campos) in datos)
        {
            var resultado = ValidarLineaProducto(linea, campos, fabricantes, skusExistentes, skusDuplicadosEnArchivo);
            if (resultado.Error is not null)
            {
                errores.Add(resultado.Error);
                continue;
            }

            productos.Add(resultado.Producto!);
            productosDto.Add(resultado.Dto!);
        }

        return (productos, productosDto, errores);
    }

    private (Producto? Producto, ProductoCargadoDto? Dto, string? Error) ValidarLineaProducto(
        int linea,
        string[] campos,
        Dictionary<int, Fabricante> fabricantes,
        List<string> skusExistentes,
        HashSet<string> skusDuplicadosEnArchivo)
    {
        if (campos.Length < 5)
            return (null, null, $"Línea {linea}: columnas insuficientes.");

        var nombre = campos[0].Trim();
        var sku = campos[1].Trim();
        var idFabricanteStr = campos[2].Trim();
        var precioStr = campos[3].Trim();
        var imagen = campos[4].Trim();

        var razones = new List<string>();

        if (string.IsNullOrWhiteSpace(nombre))
            razones.Add("el nombre está vacío");
        else if (nombre.Length > 150)
            razones.Add("el nombre excede los 150 caracteres");

        if (string.IsNullOrWhiteSpace(sku))
            razones.Add("el SKU está vacío");
        else if (sku.Length > 50)
            razones.Add("el SKU excede los 50 caracteres");
        else if (skusExistentes.Contains(sku))
            razones.Add("el SKU ya existe");
        else if (skusDuplicadosEnArchivo.Contains(sku))
            razones.Add("el SKU está duplicado en el archivo");

        if (!int.TryParse(idFabricanteStr, out var idFabricante))
            razones.Add("el ID del fabricante no es válido");
        else if (!fabricantes.ContainsKey(idFabricante))
            razones.Add("el fabricante no existe");

        if (!decimal.TryParse(precioStr, out var precio) || precio <= 0)
            razones.Add("el precio es inválido o menor o igual a cero");

        if (!Uri.IsWellFormedUriString(imagen, UriKind.Absolute))
            razones.Add("la URL de la imagen no es válida");

        if (razones.Any())
        {
            var mensaje = $"Línea {linea}: {string.Join("; ", razones)}.";
            return (null, null, mensaje);
        }

        var fabricante = fabricantes[idFabricante];

        var producto = new Producto
        {
            Nombre = nombre,
            Sku = sku,
            Precio = precio,
            ImagenUrl = imagen,
            FabricanteId = idFabricante
        };

        var dto = new ProductoCargadoDto
        {
            Nombre = nombre,
            Codigo = sku,
            Precio = precio,
            Imagen = imagen,
            FabricanteAsociado = new FabricanteDto(fabricante.Id, fabricante.RazonSocial)
        };

        return (producto, dto, null);
    }
}