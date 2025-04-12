import { Component, inject, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import {
  AlertaService,
  AuthService,
  DatosIngreso,
  SharedModule,
  TipoAlerta,
} from '@storeflow/design-system';

import { HeaderComponent } from '../../shared/header/header.component';
import { MensajesAlertas } from '../login.constantes';
import { LoginService } from '../login.service';

@Component({
  selector: 'app-admin-shell',
  standalone: true,
  imports: [SharedModule, HeaderComponent, ReactiveFormsModule],
  providers: [LoginService, AuthService],
  templateUrl: './admin-shell.component.html',
  styleUrl: './admin-shell.component.scss',
})
export class AdminShellComponent {
  mostrar = signal(true);
  service = inject(LoginService);
  router = inject(Router);
  authService = inject(AuthService);
  alertaService = inject(AlertaService);
  formulario = new FormGroup({
    correo: new FormControl('', Validators.required),
    contrasena: new FormControl('', Validators.required),
  });

  get tipoInput() {
    return this.mostrar() ? 'password' : 'text';
  }

  get iconoInput() {
    return this.mostrar() ? 'visibility_off' : 'visibility';
  }

  mostrarContrasena(event: MouseEvent) {
    this.mostrar.set(!this.mostrar());
    event.stopPropagation();
  }

  ingresar() {
    const valorFormulario = this.formulario.value as DatosIngreso;
    this.service.ingresar(valorFormulario).subscribe({
      next: ({ token }) => {
        this.authService.registrarToken(token);
        this.router.navigateByUrl('/home');
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
