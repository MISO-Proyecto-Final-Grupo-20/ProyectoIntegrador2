using System.Text.Json;
using System.Text.Json.Serialization;
using StoreFlow.Usuarios.API.Entidades;


namespace StoreFlow.Usuarios.API.DTOs;

public record UsuarioLoginRequest(DatosIngreso DatosIngreso, string TipoCategoria);

public record DatosIngreso(string Correo, string Contrasena);

public record UsuarioLoginResponse(string Token);


public class UsuarioLoginRequestConverter : JsonConverter<UsuarioLoginRequest>
{
    public override UsuarioLoginRequest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (root.TryGetProperty("correo", out JsonElement correoElement) &&
            root.TryGetProperty("contrasena", out JsonElement contrasenaElement))
        {
            var datosIngreso = new DatosIngreso(correoElement.GetString(), contrasenaElement.GetString());
            var tipoCategoria = TiposUsuarios.UsuarioCcp.ToString();
            return new UsuarioLoginRequest(datosIngreso, tipoCategoria);
        }

        if (root.TryGetProperty("datosIngreso", out JsonElement datosIngresoElement) &&
            root.TryGetProperty("tipoCategoria", out JsonElement tipoCategoriaElement))
        {
            var datosIngreso = JsonSerializer.Deserialize<DatosIngreso>(datosIngresoElement.GetRawText(), JsonSerializerOptions.Web);
            return new UsuarioLoginRequest(datosIngreso, tipoCategoriaElement.GetString());
        }

        throw new JsonException("Invalid JSON structure for UsuarioLoginRequest.");
    }
    public override void Write(Utf8JsonWriter writer, UsuarioLoginRequest value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}