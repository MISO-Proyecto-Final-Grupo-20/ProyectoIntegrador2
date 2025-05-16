import { Component } from '@angular/core';

import { RegistrarBodegaComponent } from '../registrar-bodega/registrar-bodega.component';

@Component({
  selector: 'app-compras-bodega-container',
  standalone: true,
  imports: [RegistrarBodegaComponent],
  template: `<div class="px-16 py-8 ">
    <app-registrar-bodega></app-registrar-bodega>
  </div>`,
})
export class ComprasBodegaContainerComponent {}
