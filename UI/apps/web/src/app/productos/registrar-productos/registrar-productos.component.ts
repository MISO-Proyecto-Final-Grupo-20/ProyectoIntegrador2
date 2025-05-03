import { Component, inject, signal } from '@angular/core';
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
import { ListadoFabricantes } from '../../app-model';
import { MensajesProductos } from '../productos.contantes';
import { RegistrarProducto } from '../productos.model';
import { ProductosService } from '../productos.service';
import { RegistrarProductosMasivoService } from '../registrar-productos-masivo/registrar-productos-masivo.service';

@Component({
  selector: 'app-registrar-productos',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  providers: [ProductosService, RegistrarProductosMasivoService],
  templateUrl: './registrar-productos.component.html',
})
export class RegistrarProductosComponent {
  modalRegistrarMasivo = inject(RegistrarProductosMasivoService);
  service = inject(ProductosService);
  alertaService = inject(AlertaService);
  listadoFabricantes = signal<ListadoFabricantes[]>([]);
  formulario = new FormGroup({
    nombre: new FormControl('', [
      Validators.required,
      Validators.maxLength(150),
    ]),
    fabricanteAsociado: new FormControl<number | null>(null, [
      Validators.required,
    ]),
    codigo: new FormControl<string | null>(null, [
      Validators.required,
      Validators.maxLength(50),
    ]),
    precio: new FormControl<number | null>(null, [Validators.required]),
    imagen: new FormControl<string>('', [Validators.required]),
  });

  constructor() {
    this.obtenerListadoFabricantes();
  }

  obtenerListadoFabricantes() {
    this.service.obtenerListadoFabricantes().subscribe({
      next: (fabricantes) => {
        this.listadoFabricantes.set(fabricantes);
      },
    });
  }

  guardarProducto() {
    const datosFormulario = this.formulario.value as RegistrarProducto;
    this.service.guardarProducto(datosFormulario).subscribe({
      next: () => {
        this.formulario.reset();
        this.alertaService.abrirAlerta({
          tipo: TipoAlerta.Success,
          descricion: MensajesProductos.registroProductoExitoso,
        });
      },
    });
  }

  abrirModalRegistrarMasivo() {
    this.modalRegistrarMasivo.abrirModal();
  }
}
