import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  AlertaService,
  AuthService,
  DatosIngreso,
  TipoAlerta,
  TipoCategoria,
} from '@storeflow/design-system';

import { MensajesAlertas } from '../../app.constantes';
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
  alertaService = inject(AlertaService);

  ingresar(datosIngreso: DatosIngreso) {
    this.service.ingresar(datosIngreso, TipoCategoria.Cliente).subscribe({
      next: ({ token }) => {
        this.authService.registrarToken(token);
        this.router.navigateByUrl('/home/clientes');
      },
      error: (err) => {
        if (err.status === 401) {
          this.alertaService.abrirAlerta({
            tipo: TipoAlerta.Danger,
            descricion: MensajesAlertas.credencialesIncorrectas,
          });
        }
      },
    });
  }
}
