import { Component } from '@angular/core';
import { RegistrarFabricanteComponent } from '../registrar-fabricante/registrar-fabricante.component';

@Component({
  selector: 'app-fabricantes-container',
  standalone: true,
  imports: [RegistrarFabricanteComponent],
  template: `<div class="px-16 py-8">
    <app-registrar-fabricante></app-registrar-fabricante>
  </div> `,
})
export class FabricantesContainerComponent {}
