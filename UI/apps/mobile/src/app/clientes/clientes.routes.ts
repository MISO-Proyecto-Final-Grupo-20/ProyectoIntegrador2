import { Route } from '@angular/router';
import { ConsultarPedidoComponent } from './consultar-pedido/consultar-pedido.component';
import { CrearPedidoComponent } from './crear-pedido/crear-pedido.component';
import { EntregasProgramadasComponent } from './entregas-programadas/entregas-programadas.component';
import { MenuNavegacionClientesComponent } from './menu-navegacion-clientes/menu-navegacion-clientes.component';

export const ClientesRoutes: Route[] = [
  {
    path: '',
    component: MenuNavegacionClientesComponent,
  },
  {
    path: 'crearPedido',
    component: CrearPedidoComponent,
  },
  {
    path: 'consultarPedido',
    component: ConsultarPedidoComponent,
  },
  {
    path: 'entregasProgramadas',
    component: EntregasProgramadasComponent,
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];
