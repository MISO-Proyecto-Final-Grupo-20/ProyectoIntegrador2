import { Route } from '@angular/router';
import { ClientesComponent } from './clientes/clientes.component';
import { MenuNavegacionVendedoresComponent } from './menu-navegacion-vendedores/menu-navegacion-vendedores.component';
import { RegistrarVisitaComponent } from './registrar-visita/registrar-visita.component';
import { RutasAsignadasComponent } from './rutas-asignadas/rutas-asignadas.component';

export const VendedoresRoutes: Route[] = [
  {
    path: '',
    component: MenuNavegacionVendedoresComponent,
  },
  {
    path: 'clientes',
    component: ClientesComponent,
  },
  {
    path: 'registrarVisita',
    component: RegistrarVisitaComponent,
  },
  {
    path: 'rutasAsignadas',
    component: RutasAsignadasComponent,
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];
