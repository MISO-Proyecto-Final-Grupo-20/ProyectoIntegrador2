import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@storeflow/design-system';
import { CardInformacionComponent } from '../../shared/card-informacion/card-informacion.component';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ClientesStore } from '../state';
import { ListadoProductosComponent } from '../../shared/listado-productos/listado-productos.component';

@Component({
  selector: 'app-entregas-programadas',
  standalone: true,
  imports: [
    CommonModule,
    SharedModule,
    CardInformacionComponent,
    ReactiveFormsModule,
    ListadoProductosComponent,
  ],
  templateUrl: './entregas-programadas.component.html',
})
export class EntregasProgramadasComponent {
  store = inject(ClientesStore);
  controlBuscar = new FormControl('');

  constructor() {
    this.store.obtenerEntregasProgramadas();
    this.controlBuscar.valueChanges.subscribe((valor) =>
      this.store.asignarFiltroEntregas(valor ?? '')
    );
  }

  get titulo() {
    return $localize`:@@tituloEntregas:Entregas programadas`;
  }

  get descripcion() {
    return $localize`:@@descripcionEntregas:Consulta la fecha de tu pedido y su estado`;
  }
}
