import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService, Utilidades } from '@storeflow/design-system';
import { HeaderComponent } from '../../shared/header/header.component';

@Component({
  selector: 'app-admin-shell',
  standalone: true,
  imports: [HeaderComponent, RouterOutlet],
  template: `<div class="column heigth-100">
    <app-header
      [rutaVolver]="rutaVolver"
      [mostrarAvatar]="true"
      [categoriaUsuario]="sesion.categoria"
      [nombreUsuario]="sesion.nombre"
    ></app-header>
    <div class="contenido-mobile ">
      <router-outlet></router-outlet>
    </div>
  </div> `,
})
export class AdminShellComponent {
  router = inject(Router);
  authService = inject(AuthService);
  constructor() {
    this.authService.obtenerDatosSesion();
  }

  get rutaVolver(): string {
    const url = this.router.url;

    if (
      url === '/home/clientes' ||
      url === '/home/vendedores' ||
      url === '/home'
    ) {
      return '/login';
    }

    if (url.startsWith('/home/clientes/')) {
      return '/home/clientes';
    }
    if (url.startsWith('/home/vendedores/')) {
      return '/home/vendedores';
    }
    return '/login';
  }

  get sesion() {
    return Utilidades.obtenerSesion();
  }
}
