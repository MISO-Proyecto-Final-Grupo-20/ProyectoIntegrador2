using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace StoreFlow.Usuarios.API.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class DatosSemillaUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                INSERT INTO public."Usuarios" ( "CorreoElectronico", "Contrasena", "TipoUsuario", "NombreCompleto") VALUES ('usuarioccp@correo.com', '123456', 0, 'Usuario CCP');
                INSERT INTO public."Usuarios" ( "CorreoElectronico", "Contrasena", "TipoUsuario", "NombreCompleto") VALUES ('vendedor@correo.com', '456789', 1, 'Usuario vendedor');
                INSERT INTO public."Usuarios" ( "CorreoElectronico", "Contrasena", "TipoUsuario", "NombreCompleto") VALUES ('cliente@correo.com', '789123', 2, 'Usuario cliente');
                """);
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
