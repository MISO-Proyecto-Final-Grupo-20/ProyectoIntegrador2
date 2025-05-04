using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreFlow.Ventas.API.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class CreacionPlanesDeVentasSimple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanesDeVentas",
                columns: table => new
                {
                    IdVendedor = table.Column<int>(type: "integer", nullable: false),
                    PeriodoTiempo = table.Column<byte>(type: "smallint", nullable: false),
                    MetaVentas = table.Column<decimal>(type: "numeric", nullable: false),
                    NombreVendedor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesDeVentas", x => new { x.PeriodoTiempo, x.IdVendedor });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanesDeVentas");
        }
    }
}
