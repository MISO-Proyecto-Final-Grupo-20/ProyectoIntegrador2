using System.ComponentModel.DataAnnotations;

namespace StoreFlow.Usuarios.API.Entidades;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string CorreoElectronico { get; set; } = null!;

    [Required]
    public string Contrasena { get; set; } = null!;

    [Required]
    public TiposUsuarios TipoUsuario { get; set; }

    [Required]
    public string NombreCompleto { get; set; } = null!;
    
    [MaxLength(100)]
    public string? Direccion { get; set; } = null!;
}