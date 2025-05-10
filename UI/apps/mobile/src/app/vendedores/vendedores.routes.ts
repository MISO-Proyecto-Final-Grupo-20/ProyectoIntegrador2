import { Route } from '@angular/router';
import { MenuNavegacionVendedoresComponent } from './menu-navegacion-vendedores/menu-navegacion-vendedores.component';
import { RegistrarVisitaComponent } from './registrar-visita/registrar-visita.component';
import { RutasAsignadasComponent } from './rutas-asignadas/rutas-asignadas.component';
import { CrearPedidoAClienteComponent } from './crear-pedido-a-cliente/crear-pedido-a-cliente.component';
import { ProductosContainerComponent } from './productos-container/productos-container.component';
import { PedidosPendientesContainerComponent } from './pedidos-pendientes-container/pedidos-pendientes-container.component';
import { ClientesContainerComponent } from './clientes-container/clientes-container.component';
import { ClienteSeleccionadoGuard } from './vendedores.guard';
import { VendedoresStore } from './state';
import { VendedoresService } from './services/vendedores.service';
import { ModalAgregarProductoService } from './modal-agregar-producto/modal-agregar-producto.service';
import { ConsultaClientesShellComponent } from './consulta-clientes-shell/consulta-clientes-shell.component';
import { RegistrarVisitaContainerComponent } from './registrar-visita-container/registrar-visita-container.component';

export const VendedoresRoutes: Route[] = [
  {
    path: '',
    children: [
      {
        path: '',
        component: MenuNavegacionVendedoresComponent,
      },
      {
        path: 'clientes',
        component: ClientesContainerComponent,
        children: [
          {
            path: '',
            component: ConsultaClientesShellComponent,
          },
          {
            path: 'crearPedido',
            canActivate: [ClienteSeleccionadoGuard],
            data: { redirectTo: 'home/vendedores/clientes' },
            component: CrearPedidoAClienteComponent,
            children: [
              {
                path: '',
                component: ProductosContainerComponent,
              },
              {
                path: 'pedidosPendientes',
                component: PedidosPendientesContainerComponent,
              },
            ],
          },
        ],
      },
      {
        path: 'registrarVisitas',
        children: [
          {
            path: '',
            component: RegistrarVisitaContainerComponent,
          },
          {
            path: 'cliente',
            canActivate: [ClienteSeleccionadoGuard],
            data: { redirectTo: 'home/vendedores/registrarVisitas' },
            component: RegistrarVisitaComponent,
          },
        ],
      },
      {
        path: 'rutasAsignadas',
        component: RutasAsignadasComponent,
      },
    ],

    providers: [
      VendedoresStore,
      VendedoresService,
      ModalAgregarProductoService,
    ],
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];
