import { Component, input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule, TipoCategoria } from '@storeflow/design-system';
import { DescripcionesCategoria } from '../../app.constantes';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [SharedModule, RouterModule],
  styleUrl: './header.component.scss',
  template: `<mat-toolbar class="sombra-header justify-content-between">
    <div class="row align-items-center gap-8 nowrap">
      @if (rutaVolver()) {
        <button
          data-testid="boton-volver"
          size="small"
          name="boton-volver"
          mat-icon-button
          [routerLink]="rutaVolver()"
        >
          <mat-icon class="icon-size-16 color-grey">arrow_back</mat-icon>
        </button>
      }
      <img src="assets/images/icono-header-store-flow.svg" />
    </div>
    @if (mostrarAvatar()) {
      <div
        class="row gap-4 align-items-center nowrap"
        data-testid="seccion-avatar"
      >
        <div>
          <p in18="sesion.categoria" class="color-grey-700">
            {{ descripcionCategoria }}:
          </p>
          <p class="color-grey-700">{{ nombreUsuario() }}</p>
        </div>
        <div class="avatar">
          <mat-icon>person</mat-icon>
        </div>
      </div>
    }
  </mat-toolbar> `,
})
export class HeaderComponent {
  rutaVolver = input<string>();
  mostrarAvatar = input<boolean>();
  nombreUsuario = input<string>();
  categoriaUsuario = input<TipoCategoria>();

  get descripcionCategoria(): string {
    return DescripcionesCategoria[this.categoriaUsuario() as TipoCategoria];
  }
}
