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
  selector: 'app-login-vendedor',
  standalone: true,
  imports: [HeaderComponent, LoginComponent],
  providers: [LoginService, AuthService],
  templateUrl: './login-vendedor.component.html',
})
export class LoginVendedorComponent {
  categoria = TipoCategoria.Vendedor;
  service = inject(LoginService);
  router = inject(Router);
  authService = inject(AuthService);
  ingresar(datosIngreso: DatosIngreso) {
    this.service.ingresar(datosIngreso, TipoCategoria.Vendedor).subscribe({
      next: ({ token }) => {
        this.authService.registrarToken(token);
        this.router.navigateByUrl('/home');
      },
    });
  }
}
