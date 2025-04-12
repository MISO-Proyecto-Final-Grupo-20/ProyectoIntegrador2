using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace StoreFlow.Compras.API.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class CrearFabricantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fabricantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RazonSocial = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabricantes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fabricantes_CorreoElectronico",
                table: "Fabricantes",
                column: "CorreoElectronico",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fabricantes");
        }
    }
}
