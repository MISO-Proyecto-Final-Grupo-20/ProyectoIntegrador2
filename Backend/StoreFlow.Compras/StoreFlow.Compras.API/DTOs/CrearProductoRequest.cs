using System.ComponentModel.DataAnnotations;

namespace StoreFlow.Compras.API.DTOs;

public record CrearProductoRequest(
    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres.")]
    string Nombre,
    [Required(ErrorMessage = "Debe seleccionar un fabricante asociado.")]
    int FabricanteAsociado,
    [Required(ErrorMessage = "El código del producto (SKU) es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El código (SKU) no puede tener más de 50 caracteres.")]
    string Codigo,
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un número positivo.")]
    decimal Precio,
    [Required(ErrorMessage = "La URL de la imagen es obligatoria.")]
    [RegularExpression(@"^https?:\/\/.+\..+", ErrorMessage = "La URL de la imagen no es válida.")]
    string Imagen
);