import { Component, output, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { SharedModule } from '@storeflow/design-system';
import { DatosIngreso } from '../../app.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  ingresar = output<DatosIngreso>();
  mostrar = signal(true);
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

  emitirIngresar() {
    this.ingresar.emit(this.formulario.value as DatosIngreso);
  }
}
