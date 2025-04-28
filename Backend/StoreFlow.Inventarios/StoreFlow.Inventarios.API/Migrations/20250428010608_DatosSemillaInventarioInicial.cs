using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreFlow.Inventarios.API.Migrations
{
    /// <inheritdoc />
    public partial class DatosSemillaInventarioInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO public.""Inventarios"" (""IdProducto"", ""Cantidad"")
                VALUES (1, 100),
                       (2, 100),
                       (3, 100),
                       (4, 100),
                       (5, 100),
                       (6, 100),
                       (7, 100),
                       (8, 100),
                       (9, 100),
                       (10, 100),
                       (11, 100),
                       (12, 100),
                       (13, 100),
                       (14, 100),
                       (15, 100),
                       (16, 100),
                       (17, 100),
                       (18, 100),
                       (19, 100),
                       (20, 100),
                       (21, 100),
                       (22, 100),
                       (23, 100)
                        ;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
