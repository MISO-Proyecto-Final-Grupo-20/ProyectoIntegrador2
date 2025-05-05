import { Component, inject } from '@angular/core';
import { PanelPedidosComponent } from '../../shared/panel-pedidos/panel-pedidos.component';
import { VendedoresStore } from '../state';

@Component({
  selector: 'app-pedidos-pendientes-container',
  standalone: true,
  imports: [PanelPedidosComponent],
  template: `<app-panel-pedidos
    [pedidos]="store.pedidosPendientes()"
  ></app-panel-pedidos> `,
})
export class PedidosPendientesContainerComponent {
  store = inject(VendedoresStore);

  constructor() {
    this.store.obtenerPedidosPendientesCliente();
  }
}
