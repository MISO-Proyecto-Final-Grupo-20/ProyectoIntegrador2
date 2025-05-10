import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@storeflow/design-system';
import { Pedido } from '../../app.model';
import { ConfiguracionEstadosPedidos } from '../../app.constantes';
import { ListadoProductosComponent } from '../listado-productos/listado-productos.component';

@Component({
  selector: 'app-panel-pedidos',
  standalone: true,
  imports: [SharedModule, CommonModule, ListadoProductosComponent],
  templateUrl: './panel-pedidos.component.html',
})
export class PanelPedidosComponent {
  pedidos = input<Pedido[]>([]);
  estadoPanel = new Map<number, boolean>();
  configuracionEstadosPedidos = ConfiguracionEstadosPedidos;

  abrirPanel(id: number) {
    this.estadoPanel.set(id, true);
  }

  cerrarPanel(id: number) {
    this.estadoPanel.set(id, false);
  }

  obtenerIconoPanel(id: number) {
    return this.estadoPanel.get(id) ? 'expand_less' : 'expand_more';
  }
}
