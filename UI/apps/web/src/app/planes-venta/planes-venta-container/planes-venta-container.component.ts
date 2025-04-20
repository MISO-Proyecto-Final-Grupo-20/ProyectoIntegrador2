import { Component } from '@angular/core';
import { RegistrarPlanesVentaComponent } from '../registrar-planes-ventas/registrar-planes-venta.component';

@Component({
  selector: 'app-planes-venta-container',
  standalone: true,
  imports: [RegistrarPlanesVentaComponent],
  template: `<div class="px-16 py-8">
    <app-registrar-planes-venta></app-registrar-planes-venta>
  </div>`,
})
export class PlanesVentaContainerComponent {}
