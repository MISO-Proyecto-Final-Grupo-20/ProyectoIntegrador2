import { Component, input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@storeflow/design-system';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [SharedModule, RouterModule],
  styleUrl: './header.component.scss',
  template: `<mat-toolbar class="sombra-header justify-content-between">
    <img src="assets/images/icono-header-store-flow.svg" />
    @if (nombreUsuario()) {
      <div class="row gap-4 align-items-center" data-testid="seccion-avatar">
        <p class="color-grey-700">{{ nombreUsuario() }}</p>

        <div class="avatar">
          <mat-icon>person</mat-icon>
        </div>
      </div>
    }
  </mat-toolbar> `,
})
export class HeaderComponent {
  nombreUsuario = input<string>();
}
