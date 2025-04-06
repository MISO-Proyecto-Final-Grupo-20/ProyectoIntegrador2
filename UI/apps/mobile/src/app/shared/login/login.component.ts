import { Component, input, output, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  DatosIngreso,
  SharedModule,
  TipoCategoria,
} from '@storeflow/design-system';

const CategoriaTitulo: Record<TipoCategoria, string> = {
  [TipoCategoria.Cliente]: $localize`:@@tituloCliente:Cliente`,
  [TipoCategoria.Vendedor]: $localize`:@@tituloVendedor:Vendedor`,
};

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  ingresar = output<DatosIngreso>();
  categoria = input<TipoCategoria>();
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

  get titulo() {
    return CategoriaTitulo[this.categoria() as TipoCategoria];
  }

  mostrarContrasena(event: MouseEvent) {
    this.mostrar.set(!this.mostrar());
    event.stopPropagation();
  }

  emitirIngresar() {
    this.ingresar.emit(this.formulario.value as DatosIngreso);
  }
}
