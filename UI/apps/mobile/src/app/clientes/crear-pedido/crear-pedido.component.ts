import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { SharedModule } from '@storeflow/design-system';
import { CardInformacionComponent } from '../../shared/card-informacion/card-informacion.component';
import { configuracionTabsCrearPedido } from '../clientes.constantes';
import { TabsCrearPedido } from '../clientes.model';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';

@Component({
  selector: 'app-crear-pedido',
  standalone: true,
  imports: [SharedModule, CardInformacionComponent, RouterModule],
  providers: [ClientesService],
  template: `<div class="p-16 column gap-20 heigth-100">
    <app-card-informacion
      [titulo]="titulo"
      [descripcion]="descripcion"
    ></app-card-informacion>
    <div class="flex-1 column">
      <nav
        mat-tab-nav-bar
        color="accent"
        class="tab-100 "
        [tabPanel]="tabPanel"
      >
        @for (tab of tabsCrearPedido; track $index) {
          <div
            data-testid="tab-crear-pedido"
            class="flex-1"
            name="tab"
            [active]="estaSeleccionado(tab.ruta)"
            (click)="seleccionarTab(tab)"
            mat-tab-link
          >
            {{ tab.titulo }}
          </div>
        }
      </nav>
      <mat-tab-nav-panel #tabPanel></mat-tab-nav-panel>

      <div class="flex-1">
        <router-outlet></router-outlet>
      </div>
    </div>
  </div> `,
})
export class CrearPedidoComponent {
  store = inject(ClientesStore);
  router = inject(Router);
  tabsCrearPedido = configuracionTabsCrearPedido;
  get titulo() {
    return $localize`:@@tituloCrearPedido:Crear un pedido`;
  }

  get descripcion() {
    return $localize`:@@descripcionCrearPedido:Selecciona los productos que necesitas y la cantidad`;
  }

  seleccionarTab(seleccion: TabsCrearPedido) {
    this.router.navigateByUrl(seleccion.ruta);
  }

  estaSeleccionado(url: string) {
    return this.router.url === url;
  }

  constructor() {
    this.store.obtenerProductos();
  }
}
