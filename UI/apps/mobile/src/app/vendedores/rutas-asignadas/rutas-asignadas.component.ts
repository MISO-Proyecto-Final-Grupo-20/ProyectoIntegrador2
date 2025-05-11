import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardInformacionComponent } from '../../shared/card-informacion/card-informacion.component';
import { SharedModule } from '@storeflow/design-system';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { VendedoresStore } from '../state';

@Component({
  selector: 'app-rutas-asignadas',
  standalone: true,
  imports: [
    CommonModule,
    CardInformacionComponent,
    SharedModule,
    ReactiveFormsModule,
  ],
  templateUrl: './rutas-asignadas.component.html',
  styleUrl: './rutas-asignadas.component.scss',
})
export class RutasAsignadasComponent {
  controlFecha = new FormControl(new Date());
  store = inject(VendedoresStore);
  get titulo() {
    return $localize`:@@rutasAsignadas:Rutas asignadas`;
  }

  get descripcion() {
    return $localize`:@@rutasAsignadasDescripcion:Consulta tus próximas visitas`;
  }

  constructor() {
    this.store.obtenerRutasAsignadas();
  }

  filtrarRutasAsignadas() {
    this.store.asignarFiltroRutasAsignadas(this.controlFecha.value);
  }
}
