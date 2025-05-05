import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CardInformacionComponent } from '../../shared/card-informacion/card-informacion.component';
import { configuracionTabsCrearPedido } from '../clientes.constantes';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';
import { TabsComponent } from '../../shared/tabs/tabs.component';

@Component({
  selector: 'app-crear-pedido',
  standalone: true,
  imports: [CardInformacionComponent, RouterModule, TabsComponent],
  providers: [ClientesService],
  template: `<div class="p-16 column gap-20 heigth-100">
    <app-card-informacion
      [titulo]="titulo"
      [descripcion]="descripcion"
    ></app-card-informacion>
    <div class="flex-1 column">
      <app-tabs [tabs]="tabsCrearPedido"></app-tabs>
      <div class="flex-1">
        <router-outlet></router-outlet>
      </div>
    </div>
  </div> `,
})
export class CrearPedidoComponent {
  store = inject(ClientesStore);
  tabsCrearPedido = configuracionTabsCrearPedido;
  get titulo() {
    return $localize`:@@tituloCrearPedido:Crear un pedido`;
  }

  get descripcion() {
    return $localize`:@@descripcionCrearPedido:Selecciona los productos que necesitas y la cantidad`;
  }

  constructor() {
    this.store.obtenerProductos();
  }
}
