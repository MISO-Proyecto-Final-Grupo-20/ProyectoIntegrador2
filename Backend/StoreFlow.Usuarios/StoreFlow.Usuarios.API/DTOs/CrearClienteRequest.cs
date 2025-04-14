using System.ComponentModel.DataAnnotations;

namespace StoreFlow.Usuarios.API.DTOs;

public record CrearClienteRequest(
    string? Nombre,
    string? Correo,
    string? Direccion,
    string? Contrasena);

public static class CrearClienteRequestValidator
{
    public static void Validar(this CrearClienteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            throw new ArgumentException("El nombre es obligatorio");
        
        if (request.Nombre.Length > 100)
            throw new ArgumentException("El nombre no puede exceder los 100 caracteres");
        
        if (string.IsNullOrWhiteSpace(request.Correo))
            throw new ArgumentException("El correo es obligatorio");
        
        if (!new EmailAddressAttribute().IsValid(request.Correo))
            throw new ArgumentException("El correo no tiene un formato válido");
        
        if (request.Correo.Length > 100)
            throw new ArgumentException("El correo no puede tener más de 100 caracteres");
        
        if (string.IsNullOrWhiteSpace(request.Contrasena))
            throw new ArgumentException("La contraseña es obligatoria");
        
        if (request.Contrasena.Length < 8)
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres");
    }
}