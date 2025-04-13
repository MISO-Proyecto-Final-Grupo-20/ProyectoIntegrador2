import { Component, input, output, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterModule } from '@angular/router';
import {
  DatosIngreso,
  SharedModule,
  TipoCategoria,
} from '@storeflow/design-system';

interface ConfiguracionCategoria {
  titulo: string;
  ruta?: string;
}
const ConfiguracionCategoria: Record<TipoCategoria, ConfiguracionCategoria> = {
  [TipoCategoria.Cliente]: {
    titulo: $localize`:@@tituloCliente:Cliente`,
    ruta: '/login/registroCliente',
  },
  [TipoCategoria.Vendedor]: { titulo: $localize`:@@tituloVendedor:Vendedor` },
};

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule, RouterModule],
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

  get configuracion() {
    return ConfiguracionCategoria[this.categoria() as TipoCategoria];
  }

  mostrarContrasena(event: MouseEvent) {
    this.mostrar.set(!this.mostrar());
    event.stopPropagation();
  }

  emitirIngresar() {
    this.ingresar.emit(this.formulario.value as DatosIngreso);
  }
}
