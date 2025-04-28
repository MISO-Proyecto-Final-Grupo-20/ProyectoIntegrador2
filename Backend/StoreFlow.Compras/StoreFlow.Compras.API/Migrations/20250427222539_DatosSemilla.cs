using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreFlow.Compras.API.Migrations
{
    [ExcludeFromCodeCoverage]

    /// <inheritdoc />
    public partial class DatosSemilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.Sql(@"
                INSERT INTO public.""Fabricantes"" (""RazonSocial"", ""CorreoElectronico"")
                VALUES 
                    ('Fabricante 1', 'correo@fabricante1.com'),
                    ('Fabricante 2', 'correo@fabricante2.com'),
                    ('Fabricante 3', 'correo@fabricante3.com');");


            migrationBuilder.Sql(@"
                INSERT INTO public.""Productos"" (""Nombre"", ""Sku"", ""Precio"", ""ImagenUrl"", ""FabricanteId"")
                VALUES ('Nutella', '3017620429484', 45300, 'https://images.openfoodfacts.org/images/products/301/762/042/9484/front_en.373.200.jpg', 1)
                , ('Chocolate 85 porciento cacao', '8410109109832', 11400, 'https://images.openfoodfacts.org/images/products/841/010/910/9832/front_en.46.200.jpg', 1)
                , ('Kola Granulada Tarrito Rojo', '7702560026102', 20500, 'https://images.openfoodfacts.org/images/products/770/256/002/6102/front_es.3.200.jpg', 1)
                , ('Old Fashioned Rolled Oats', '0039978033758', 30000, 'https://images.openfoodfacts.org/images/products/003/997/803/3758/front_en.43.200.jpg', 1)
                , ('82% Cacao Supreme Dark', '8410109104950', 29300, 'https://images.openfoodfacts.org/images/products/841/010/910/4950/front_es.30.200.jpg', 1)
                , ('Noix de cajou', '20245290', 15600, 'https://images.openfoodfacts.org/images/products/000/002/024/5290/front_fr.6.200.jpg', 1)
                , ('Chocolate Negro 70% 0% Azúcares Añadidos', '8410109050882', 14000, 'https://images.openfoodfacts.org/images/products/841/010/905/0882/front_en.27.200.jpg', 1)
                , ('Dark Chocolate 70% Cacao', '8410109050509', 17700, 'https://images.openfoodfacts.org/images/products/841/010/905/0509/front_en.18.200.jpg', 1)
                , ('Tratamiento Mais Cachos', '7897042012312', 41800, 'https://images.openfoodfacts.org/images/products/789/704/201/2312/front_es.5.200.jpg', 1)
                , ('Pan Integral Dextrin Tradicional', '8412170026896', 42500, 'https://images.openfoodfacts.org/images/products/841/217/002/6896/front_es.25.200.jpg', 1)
                , ('Lentilles', '3222471623718', 37500, 'https://images.openfoodfacts.org/images/products/322/247/162/3718/front_fr.31.200.jpg', 1)
                , ('Crema de Maní', '7702007065411', 32100, 'https://images.openfoodfacts.org/images/products/770/200/706/5411/front_en.4.200.jpg', 1)
                , ('PAN Harina De Maiz Blanco', '7702084137520', 32700, 'https://images.openfoodfacts.org/images/products/770/208/413/7520/front_es.41.200.jpg', 1)
                , ('Bizcochitos', '7792180139719', 45300, 'https://images.openfoodfacts.org/images/products/779/218/013/9719/front_es.9.200.jpg', 1)
                , ('Club Social Integral Tradicional', '7750168001694', 28000, 'https://images.openfoodfacts.org/images/products/775/016/800/1694/front_es.18.200.jpg', 1)
                , ('Noglut Maria', '8412170011434', 37700, 'https://images.openfoodfacts.org/images/products/841/217/001/1434/front_en.29.200.jpg', 1)
                , ('Valorcao a la taza', '8410109113051', 18800, 'https://images.openfoodfacts.org/images/products/841/010/911/3051/front_es.16.200.jpg', 1)
                , ('Ratatouille à la provenzale', '3222471055113', 36700, 'https://images.openfoodfacts.org/images/products/322/247/105/5113/front_fr.38.200.jpg', 1)
                , ('Oreo Original', '7590011251100', 46600, 'https://images.openfoodfacts.org/images/products/759/001/125/1100/front_es.36.200.jpg', 1)
                , ('Nescafé Gold', '7613031154111', 28200, 'https://images.openfoodfacts.org/images/products/761/303/115/4111/front_es.13.200.jpg', 1)
                , ('Chicharrón Americano Jacks', '7702189045775', 10700, 'https://images.openfoodfacts.org/images/products/770/218/904/5775/front_en.10.200.jpg', 1)
                , ('Miel', '7702025100156', 19500, 'https://images.openfoodfacts.org/images/products/770/202/510/0156/front_en.64.200.jpg', 1)
                , ('Gomiti 53', '8005121000535', 13200, 'https://images.openfoodfacts.org/images/products/800/512/100/0535/front_it.30.200.jpg', 1)
                ;"
);


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
