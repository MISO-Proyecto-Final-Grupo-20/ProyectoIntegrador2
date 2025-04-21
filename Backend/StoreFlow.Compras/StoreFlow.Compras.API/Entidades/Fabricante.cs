using System.ComponentModel.DataAnnotations;

namespace StoreFlow.Compras.API.Entidades;

public class Fabricante
{
    [Key] public int Id { get; set; }

    [Required(ErrorMessage = "La razón social es obligatoria")]
    [MaxLength(100, ErrorMessage = "La razón social no puede exceder los 100 caracteres")]
    public required string RazonSocial { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
    [MaxLength(100, ErrorMessage = "El correo no puede tener más de 100 caracteres")]

    public required string CorreoElectronico { get; set; }


    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}