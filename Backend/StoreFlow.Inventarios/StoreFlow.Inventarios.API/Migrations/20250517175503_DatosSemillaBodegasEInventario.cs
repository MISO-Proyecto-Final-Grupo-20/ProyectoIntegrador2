using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreFlow.Inventarios.API.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class DatosSemillaBodegasEInventario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insertar bodegas
            migrationBuilder.Sql(@"
            INSERT INTO public.""Bodegas"" (""Id"", ""Nombre"")
            VALUES 
                (1, 'Norte'),
                (2, 'Centro'),
                (3, 'Sur');

            SELECT setval(pg_get_serial_sequence('""Bodegas""', 'Id'), 3, true);
        ");

            // Insertar inventario para la bodega 1
            migrationBuilder.Sql(@"
        INSERT INTO public.""Inventarios"" (""IdProducto"", ""IdBodega"", ""Cantidad"")
        VALUES 
            (1, 1, 100),
            (2, 1, 100),
            (3, 1, 100),
            (4, 1, 100),
            (5, 1, 100),
            (6, 1, 100),
            (7, 1, 100),
            (8, 1, 100),
            (9, 1, 100),
            (10, 1, 100),
            (11, 1, 100),
            (12, 1, 100),
            (13, 1, 100),
            (14, 1, 100),
            (15, 1, 100),
            (16, 1, 100),
            (17, 1, 100),
            (18, 1, 100),
            (19, 1, 100),
            (20, 1, 100),
            (21, 1, 100),
            (22, 1, 100),
            (23, 1, 100);
    ");
        }
        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM public.""Inventarios"";");
            migrationBuilder.Sql(@"DELETE FROM public.""Bodegas"";");
        }
    }
}
