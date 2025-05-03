import { Component, inject } from '@angular/core';
import { PanelPedidosComponent } from '../../shared/panel-pedidos/panel-pedidos.component';
import { ClientesStore } from '../state';

@Component({
  selector: 'app-pedidos-pendientes',
  standalone: true,
  imports: [PanelPedidosComponent],
  template: `<app-panel-pedidos
    [pedidos]="store.pedidosPendientes()"
  ></app-panel-pedidos> `,
})
export class PedidosPendientesComponent {
  store = inject(ClientesStore);
  constructor() {
    this.store.obtenerPedidos();
  }
}
