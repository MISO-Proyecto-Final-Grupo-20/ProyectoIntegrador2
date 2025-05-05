import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { VendedoresStore } from '../state';
import { TabsComponent } from '../../shared/tabs/tabs.component';
import { configuracionTabsCrearPedidoACliente } from '../vendedores.constantes';

@Component({
  selector: 'app-crear-pedido-a-cliente',
  standalone: true,
  imports: [RouterModule, TabsComponent],
  template: `<div class="p-16 column gap-20 heigth-100">
    <h3 class="color-primary">{{ store.clienteSeleccionado()?.nombre }}</h3>
    <div class="flex-1 column">
      <app-tabs [tabs]="tabsCrearPedido"></app-tabs>
      <div class="flex-1">
        <router-outlet></router-outlet>
      </div>
    </div>
  </div> `,
})
export class CrearPedidoAClienteComponent {
  store = inject(VendedoresStore);
  tabsCrearPedido = configuracionTabsCrearPedidoACliente;
}
