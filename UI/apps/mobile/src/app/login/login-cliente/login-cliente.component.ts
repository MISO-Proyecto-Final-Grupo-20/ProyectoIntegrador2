import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  AuthService,
  DatosIngreso,
  TipoCategoria,
} from '@storeflow/design-system';
import { HeaderComponent } from '../../shared/header/header.component';
import { LoginComponent } from '../../shared/login/login.component';
import { LoginService } from '../login.service';

@Component({
  selector: 'app-login-cliente',
  standalone: true,
  imports: [LoginComponent, HeaderComponent],
  providers: [LoginService, AuthService],
  template: `<div class="column heigth-100">
    <app-header rutaVolver="/"></app-header>
    <div class="contenido-mobile column">
      <app-login
        data-testid="app-login"
        class="flex-1 column"
        (ingresar)="ingresar($event)"
        [categoria]="categoria"
      ></app-login>
    </div>
  </div> `,
})
export class LoginClienteComponent {
  categoria = TipoCategoria.Cliente;
  service = inject(LoginService);
  router = inject(Router);
  authService = inject(AuthService);

  ingresar(datosIngreso: DatosIngreso) {
    this.service.ingresar(datosIngreso, TipoCategoria.Cliente).subscribe({
      next: ({ token }) => {
        this.authService.registrarToken(token);
        this.router.navigateByUrl('/home');
      },
    });
  }
}
