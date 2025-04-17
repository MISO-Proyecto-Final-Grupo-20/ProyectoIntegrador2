import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SharedModule } from '@storeflow/design-system';
import { listadoMenuNavegacion } from '../home.constantes';
import { MenuNavegacion } from '../home.model';

@Component({
  selector: 'app-menu-navegacion',
  standalone: true,
  imports: [SharedModule],
  template: `<div class="bg-white heigth-100">
    <mat-selection-list>
      @for (item of listadoMenuNavegacion; track $index) {
        <mat-list-item
          data-testid="menu-navegacion"
          (click)="seleccionarOpcion(item)"
          [class.bg-grey-100]="obtenerSiEstaSeleccionado(item)"
        >
          <div
            class="row align-items-center gap-8 color-grey-600 px-16"
            [class.color-primary]="obtenerSiEstaSeleccionado(item)"
          >
            <mat-icon class="mat-icon-outlined">{{ item.iconoLista }}</mat-icon>
            <p class="mat-body-1">{{ item.titulo }}</p>
          </div>
        </mat-list-item>
      }
    </mat-selection-list>
  </div> `,
})
export class MenuNavegacionComponent {
  router = inject(Router);
  listadoMenuNavegacion = listadoMenuNavegacion;
  opcionSeleccionada: MenuNavegacion = listadoMenuNavegacion[0];

  obtenerSiEstaSeleccionado(opcion: MenuNavegacion) {
    return this.router.url === `/home${opcion.url}`;
  }

  seleccionarOpcion(opcion: MenuNavegacion) {
    this.opcionSeleccionada = opcion;
    this.router.navigateByUrl(`/home${opcion.url}`);
  }
}
