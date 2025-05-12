using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreFlow.Ventas.API.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class AgregarNombreYTamanoVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreOriginal",
                table: "Videos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TamanioBytes",
                table: "Videos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreOriginal",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "TamanioBytes",
                table: "Videos");
        }
    }
}
