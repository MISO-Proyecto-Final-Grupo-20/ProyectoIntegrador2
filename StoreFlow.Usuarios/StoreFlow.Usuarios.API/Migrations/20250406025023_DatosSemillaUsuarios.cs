using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreFlow.Usuarios.API.Migrations
{
    /// <inheritdoc />
    public partial class DatosSemillaUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                INSERT INTO public."Usuarios" ("Id", "CorreoElectronico", "Contrasena", "TipoUsuario", "NombreCompleto") VALUES (1, 'usuarioccp@correo.com', '123456', 0, 'Usuario CCP');
                INSERT INTO public."Usuarios" ("Id", "CorreoElectronico", "Contrasena", "TipoUsuario", "NombreCompleto") VALUES (2, 'vendedor@correo.com', '456789', 1, 'Usuario vendedor');
                INSERT INTO public."Usuarios" ("Id", "CorreoElectronico", "Contrasena", "TipoUsuario", "NombreCompleto") VALUES (3, 'cliente@correo.com', '789123', 2, 'Usuario cliente');
                """);
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
