import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  AlertaService,
  SharedModule,
  TipoAlerta,
} from '@storeflow/design-system';
import { MensajesVendedores } from '../vendedores.constantes';
import { RegistrarVendedor } from '../vendedores.model';
import { VendedoresService } from '../vendedores.service';

@Component({
  selector: 'app-registrar-vendedores',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  providers: [VendedoresService],
  templateUrl: './registrar-vendedores.component.html',
})
export class RegistrarVendedoresComponent {
  service = inject(VendedoresService);
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
    contrasena: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
  });

  guardarVendedor() {
    const datosFormulario = this.formulario.value as RegistrarVendedor;
    this.service.registrarVendedor(datosFormulario).subscribe({
      next: () => {
        this.formulario.reset();
        this.alertaService.abrirAlerta({
          tipo: TipoAlerta.Success,
          descricion: MensajesVendedores.registrarVendedorExitoso,
        });
      },
    });
  }
}
