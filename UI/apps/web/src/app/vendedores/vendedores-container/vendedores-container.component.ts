import { Component } from '@angular/core';
import { RegistrarVendedoresComponent } from '../registrar-vendedores/registrar-vendedores.component';

@Component({
  selector: 'app-vendedores-container',
  standalone: true,
  imports: [RegistrarVendedoresComponent],
  template: `<div class="px-16 py-8">
    <app-registrar-vendedores></app-registrar-vendedores>
  </div> `,
})
export class VendedoresContainerComponent {}
