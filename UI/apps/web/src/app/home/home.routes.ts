import { Route } from '@angular/router';
import { AdminShellComponent } from './admin-shell/admin-shell.component';

export const HomeRoutes: Route[] = [
  {
    path: '',
    component: AdminShellComponent,
    children: [
      {
        path: 'fabricantes',
        loadChildren: () =>
          import('../fabricantes/fabricantes.routes').then(
            (m) => m.FabricantesRoutes
          ),
      },
      {
        path: 'productos',
        loadChildren: () =>
          import('../productos/productos.routes').then(
            (m) => m.ProductosRoutes
          ),
      },
      {
        path: 'vendedores',
        loadChildren: () =>
          import('../vendedores/vendedores.routes').then(
            (m) => m.VendedoresRoutes
          ),
      },
      {
        path: 'planesVenta',
        loadChildren: () =>
          import('../planes-venta/planes-venta.routes').then(
            (m) => m.PlanesVentaRoutes
          ),
      },
      {
        path: 'informes',
        loadChildren: () =>
          import('../informes/informes.routes').then((m) => m.InformesRoutes),
      },
      {
        path: 'comprasBodega',
        loadChildren: () =>
          import('../compras-bodega/compras-bodega.routes').then(
            (m) => m.ComprasBodegaRoutes
          ),
      },
      {
        path: 'analisisTiendas',
        loadChildren: () =>
          import('../analisis-tiendas/analisis-tiendas.routes').then(
            (m) => m.AnalisisTiendasRoutes
          ),
      },
      {
        path: 'programacionRutas',
        loadChildren: () =>
          import('../programacion-rutas/programacion-rutas.routes').then(
            (m) => m.ProgramacionRutasRoutes
          ),
      },
      {
        path: '**',
        redirectTo: 'fabricantes',
        pathMatch: 'full',
      },
    ],
  },
];
