import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { SharedModule } from '@storeflow/design-system';
import { ComprasBodegaStore } from '../state';
import { ModalSeleccionarProductosService } from '../modal-seleccionar-productos/modal-seleccionar-productos.service';
import { FormularioRegistroBodega } from '../compras-bodega.model';

@Component({
  selector: 'app-registrar-bodega',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  providers: [ModalSeleccionarProductosService],
  templateUrl: './registrar-bodega.component.html',
  styleUrl: './registrar-bodega.component.scss',
})
export class RegistrarBodegaComponent {
  store = inject(ComprasBodegaStore);
  modalSeleccionarProductosService = inject(ModalSeleccionarProductosService);
  formulario = new FormGroup({
    fabricante: new FormControl<number | null>(null, Validators.required),
    bodega: new FormControl<number | null>(null, Validators.required),
  });

  get desabilitarBotonRegistrar(): boolean {
    return (
      this.formulario.invalid ||
      this.store.productosSeleccionados().length === 0
    );
  }

  constructor() {
    this.store.cargarDatos();
  }

  abrirModalSeleccionarProductos() {
    this.modalSeleccionarProductosService.abrirModal();
  }

  registrarCompraBodega() {
    const formulario = this.formulario.value as FormularioRegistroBodega;
    this.store.registrarCompraBodega({
      ...formulario,
      productos: this.store.productosSeleccionadosRegistro(),
    });
    this.formulario.reset();
  }
}
