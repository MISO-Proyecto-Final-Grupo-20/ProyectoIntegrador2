import { Component, inject } from '@angular/core';
import { ClientesStore } from '../state';
import { CardInformacionComponent } from '../../shared/card-informacion/card-informacion.component';
import { SharedModule } from '@storeflow/design-system';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { PanelPedidosComponent } from '../../shared/panel-pedidos/panel-pedidos.component';

@Component({
  selector: 'app-consultar-pedido',
  standalone: true,
  imports: [
    CardInformacionComponent,
    SharedModule,
    ReactiveFormsModule,
    PanelPedidosComponent,
  ],
  templateUrl: './consultar-pedido.component.html',
  styleUrl: './consultar-pedido.component.scss',
})
export class ConsultarPedidoComponent {
  store = inject(ClientesStore);
  controlBuscar = new FormControl('');

  constructor() {
    this.store.asignarFiltroPedidos('');
    this.controlBuscar.valueChanges.subscribe((valor) =>
      this.store.asignarFiltroPedidos(valor)
    );
    this.store.obtenerPedidos();
  }

  get titulo() {
    return $localize`:@@tituloConsultarPedido:Consultar pedido`;
  }

  get descripcion() {
    return $localize`:@@descripcionConsultarPedido:Aquí puedes encontrar todos tus pedidos y sus estados`;
  }
}
