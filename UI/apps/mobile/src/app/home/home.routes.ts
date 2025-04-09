import { Route } from '@angular/router';
import { CategoriaGuard } from '../categoria.guard';
import { AdminShellComponent } from './admin-shell/admin-shell.component';

export const HomeRoutes: Route[] = [
  {
    path: '',
    component: AdminShellComponent,
    children: [
      {
        canActivate: [CategoriaGuard],
        path: 'clientes',
        loadChildren: () =>
          import('../clientes/clientes.routes').then((m) => m.ClientesRoutes),
      },
      {
        canActivate: [CategoriaGuard],
        path: 'vendedores',
        loadChildren: () =>
          import('../vendedores/vendedores.routes').then(
            (m) => m.VendedoresRoutes
          ),
      },
    ],
  },
];
