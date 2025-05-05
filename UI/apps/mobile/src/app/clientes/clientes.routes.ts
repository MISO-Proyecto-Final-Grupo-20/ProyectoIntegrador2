import { Route } from '@angular/router';
import { ConsultarPedidoComponent } from './consultar-pedido/consultar-pedido.component';
import { CrearPedidoComponent } from './crear-pedido/crear-pedido.component';
import { EntregasProgramadasComponent } from './entregas-programadas/entregas-programadas.component';
import { MenuNavegacionClientesComponent } from './menu-navegacion-clientes/menu-navegacion-clientes.component';
import { ModalAgregarProductoService } from './modal-agregar-producto/modal-agregar-producto.service';
import { PedidosPendientesComponent } from './pedidos-pendientes/pedidos-pendientes.component';
import { ClientesService } from './services/clientes.service';
import { ClientesStore } from './state';
import { ProductosContainerComponent } from './productos-container/productos-container.component';

export const ClientesRoutes: Route[] = [
  {
    path: '',
    children: [
      {
        path: '',
        component: MenuNavegacionClientesComponent,
      },
      {
        path: 'crearPedido',
        component: CrearPedidoComponent,
        children: [
          {
            path: '',
            component: ProductosContainerComponent,
          },
          {
            path: 'pedidosPendientes',
            component: PedidosPendientesComponent,
          },
        ],
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
    ],
    providers: [ClientesStore, ClientesService, ModalAgregarProductoService],
  },
];
