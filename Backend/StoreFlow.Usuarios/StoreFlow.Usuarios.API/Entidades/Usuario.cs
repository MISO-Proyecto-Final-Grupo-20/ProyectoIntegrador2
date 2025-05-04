using System.ComponentModel.DataAnnotations;
using StoreFlow.Usuarios.API.DTOs;

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

    public ClienteResponse ConvertirAClienteResponse()
    {
        return new ClienteResponse(Id, NombreCompleto, Direccion ?? "Sin Dirección registrada.");
    }
    
    public VendedorResponse ConvertirAVendedorResponse()
    {
        return new VendedorResponse(Id, NombreCompleto, CorreoElectronico);
    }
    
}