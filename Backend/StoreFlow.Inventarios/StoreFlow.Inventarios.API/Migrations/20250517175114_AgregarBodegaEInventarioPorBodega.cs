using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StoreFlow.Inventarios.API.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class AgregarBodegaEInventarioPorBodega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventarios",
                table: "Inventarios");

            migrationBuilder.AddColumn<int>(
                name: "IdBodega",
                table: "Inventarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventarios",
                table: "Inventarios",
                columns: new[] { "IdProducto", "IdBodega" });

            migrationBuilder.CreateTable(
                name: "Bodegas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodegas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventarios_IdBodega",
                table: "Inventarios",
                column: "IdBodega");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventarios_Bodegas_IdBodega",
                table: "Inventarios",
                column: "IdBodega",
                principalTable: "Bodegas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventarios_Bodegas_IdBodega",
                table: "Inventarios");

            migrationBuilder.DropTable(
                name: "Bodegas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventarios",
                table: "Inventarios");

            migrationBuilder.DropIndex(
                name: "IX_Inventarios_IdBodega",
                table: "Inventarios");

            migrationBuilder.DropColumn(
                name: "IdBodega",
                table: "Inventarios");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventarios",
                table: "Inventarios",
                column: "IdProducto");
        }
    }
}
