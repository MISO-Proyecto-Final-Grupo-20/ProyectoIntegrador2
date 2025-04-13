import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService, Utilidades } from '@storeflow/design-system';
import { HeaderComponent } from '../../shared/header/header.component';
import { MenuNavegacionComponent } from '../menu-navegacion/menu-navegacion.component';

@Component({
  selector: 'app-admin-shell',
  standalone: true,
  imports: [HeaderComponent, MenuNavegacionComponent, RouterOutlet],
  template: `<div class="column heigth-100">
    <app-header [nombreUsuario]="sesion.nombreUsuario"></app-header>
    <div class="contenido-web">
      <div class="row heigth-100">
        <app-menu-navegacion
          class="width-280 heigth-100 menu"
        ></app-menu-navegacion>
        <div class="flex-1"><router-outlet></router-outlet></div>
      </div>
    </div>
  </div> `,
  styleUrl: './admin-shell.component.scss',
})
export class AdminShellComponent {
  authService = inject(AuthService);
  constructor() {
    this.authService.obtenerDatosSesion();
  }

  get sesion() {
    return Utilidades.obtenerSesion();
  }
}
