using System.ComponentModel.DataAnnotations;

namespace StoreFlow.Compras.API.DTOs
{
    public record CrearFabricanteRequest(
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        string Nombre,
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
        [MaxLength(100, ErrorMessage = "El correo no puede tener más de 100 caracteres")]
        string CorreoElectronico
        );
}