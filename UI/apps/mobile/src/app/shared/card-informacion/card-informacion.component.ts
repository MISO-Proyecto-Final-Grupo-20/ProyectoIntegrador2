import { Component, input } from '@angular/core';

@Component({
  selector: 'app-card-informacion',
  standalone: true,
  template: `<div class="p-12 bg-grey-100 radius-4">
    <h3 class="color-primary">{{ titulo() }}</h3>
    <p class="color-on-surface">
      {{ descripcion() }}
    </p>
  </div> `,
})
export class CardInformacionComponent {
  titulo = input<string>();
  descripcion = input<string>();
}
