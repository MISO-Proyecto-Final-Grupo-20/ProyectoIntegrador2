import { Component, inject } from '@angular/core';
import { SharedModule, Utilidades } from '@storeflow/design-system';
import { VendedoresStore } from '../state';
import { AdjuntarEvidenciaService } from '../adjuntar-evidencia/adjuntar-evidencia.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegistrarVisita } from '../vendedores.model';

@Component({
  selector: 'app-registrar-visita',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule, CommonModule],
  providers: [AdjuntarEvidenciaService],
  templateUrl: './registrar-visita.component.html',
  styleUrls: ['./registrar-visita.component.scss'],
})
export class RegistrarVisitaComponent {
  store = inject(VendedoresStore);
  adjuntarEvidenciaService = inject(AdjuntarEvidenciaService);
  formulario = new FormGroup({
    fecha: new FormControl<Date | null>(null, Validators.required),
    hora: new FormControl<string | null>(null, Validators.required),
  });

  constructor() {
    this.store.obtenerRegistroVisitas();
  }

  abrirModalAdjuntar() {
    this.adjuntarEvidenciaService.abrirModal();
  }

  get botonEstaDesabilitado() {
    return this.formulario.invalid || !this.store.archivoSeleccionado();
  }

  registrarVisitar() {
    const { fecha, hora } = this.formulario.value;
    const datosVisita: RegistrarVisita = {
      hora,
      fecha,
      archivo: this.store.archivoSeleccionado(),
    } as RegistrarVisita;
    this.store.registrarVisita(datosVisita);
    this.formulario.reset();
  }

  obtenerHoraComoFecha(hora: string) {
    return Utilidades.obtenerHoraComoFecha(hora);
  }

  obtenerTamanioArchivo(tamanio: number | undefined) {
    return Utilidades.obtenerTamanioArchivo(tamanio as number);
  }
}
