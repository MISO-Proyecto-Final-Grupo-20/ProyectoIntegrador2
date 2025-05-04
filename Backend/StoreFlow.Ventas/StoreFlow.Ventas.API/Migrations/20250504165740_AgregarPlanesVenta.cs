using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StoreFlow.Ventas.API.Migrations
{
    public partial class AgregarPlanesVenta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeriodosTiempo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodosTiempo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanesVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Precio = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PeriodoTiempoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanesVenta_PeriodosTiempo_PeriodoTiempoId",
                        column: x => x.PeriodoTiempoId,
                        principalTable: "PeriodosTiempo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanesVenta_PeriodoTiempoId",
                table: "PlanesVenta",
                column: "PeriodoTiempoId");

            migrationBuilder.InsertData(
                table: "PeriodosTiempo",
                columns: ["Id", "Nombre"],
                values: new object[,]
                {
                    { 1, "Mensual" },
                    { 2, "Trimestral" },
                    { 3, "Semestral" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanesVenta");

            migrationBuilder.DropTable(
                name: "PeriodosTiempo");
        }
    }
}
