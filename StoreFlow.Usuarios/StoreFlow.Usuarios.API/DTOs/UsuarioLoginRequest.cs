namespace StoreFlow.Usuarios.API.DTOs;

public record UsuarioLoginRequest(DatosIngreso DatosIngreso, string TipoCategoria);

public record DatosIngreso(string Correo, string Contrasena);

public record UsuarioLoginResponse(string Token);

