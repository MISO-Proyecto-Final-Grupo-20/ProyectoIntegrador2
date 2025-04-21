using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreFlow.Compras.API.Entidades
{
    public class Producto
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [MaxLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El código del producto (SKU) puede tener más de 50 caracteres")]
        [MaxLength(50)]
        public required string Sku { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La url del producto es requerida")]
        [Url]
        public required string ImagenUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Fabricante))]
        public int FabricanteId { get; set; }

        public Fabricante Fabricante { get; set; } = null!;
    }
}