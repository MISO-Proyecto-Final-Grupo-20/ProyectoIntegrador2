using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreFlow.Ventas.API.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class PedidosInformacionProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "ProductosPedidos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Imagen",
                table: "ProductosPedidos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "ProductosPedidos",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "ProductosPedidos");

            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "ProductosPedidos");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "ProductosPedidos");
        }
    }
}
