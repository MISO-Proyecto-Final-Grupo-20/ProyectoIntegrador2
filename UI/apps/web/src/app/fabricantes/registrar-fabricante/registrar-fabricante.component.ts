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
import { Fabricante } from '../fabricante.model';
import { MensajesFabricantes } from '../fabricantes.constantes';
import { FabricantesService } from '../fabricantes.service';

@Component({
  selector: 'app-registrar-fabricante',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  providers: [FabricantesService],
  templateUrl: './registrar-fabricante.component.html',
})
export class RegistrarFabricanteComponent {
  service = inject(FabricantesService);
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
  });

  guardarFabricante() {
    const datosFormulario = this.formulario.value as Fabricante;
    this.service.guardarFabricante(datosFormulario).subscribe({
      next: () => {
        this.formulario.reset();
        this.alertaService.abrirAlerta({
          tipo: TipoAlerta.Success,
          descricion: MensajesFabricantes.registroFabricanteExitoso,
        });
      },
    });
  }
}
