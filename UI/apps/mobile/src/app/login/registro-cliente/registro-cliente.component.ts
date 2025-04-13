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
  SharedModule,
  TipoAlerta,
} from '@storeflow/design-system';
import { MensajesAlertas } from '../../app.constantes';
import { HeaderComponent } from '../../shared/header/header.component';
import { Cliente } from '../login.model';
import { LoginService } from '../login.service';

@Component({
  selector: 'app-registro-cliente',
  standalone: true,
  imports: [HeaderComponent, SharedModule, ReactiveFormsModule],
  providers: [LoginService],
  templateUrl: './registro-cliente.component.html',
})
export class RegistroClienteComponent {
  mostrar = signal(true);
  service = inject(LoginService);
  router = inject(Router);
  alertaService = inject(AlertaService);

  formulario = new FormGroup({
    nombre: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
    ]),
    correo: new FormControl('', [
      Validators.required,
      Validators.email,
      Validators.maxLength(100),
    ]),
    direccion: new FormControl('', [
      Validators.required,
      Validators.maxLength(200),
    ]),
    contrasena: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.maxLength(200),
      Validators.minLength(8),
      Validators.pattern(
        /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+[\]{};':"\\|,.<>/?`~-]).*$/
      ),
    ]),
  });

  get contrasenaInvalidad() {
    return (
      this.formulario.get('contrasena')?.invalid &&
      this.formulario.get('contrasena')?.touched
    );
  }

  get iconoInput() {
    return this.mostrar() ? 'visibility_off' : 'visibility';
  }

  get tipoInput() {
    return this.mostrar() ? 'password' : 'text';
  }

  mostrarContrasena(event: MouseEvent) {
    this.mostrar.set(!this.mostrar());
    event.stopPropagation();
  }

  guardarCliente() {
    const datosFormulario = this.formulario.value as Cliente;
    this.service.registrarCliente(datosFormulario).subscribe({
      next: () => {
        this.alertaService.abrirAlerta({
          tipo: TipoAlerta.Success,
          descricion: MensajesAlertas.clienteRegistradoExitoso,
        });
        this.router.navigateByUrl('/login/cliente');
      },
    });
  }
}
