import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  AlertaService,
  Archivo,
  SharedModule,
  Utilidades,
} from '@storeflow/design-system';
import { AnalisisTiendasService } from '../analisis-tiendas.service';
import { take } from 'rxjs';
import {
  AnalisisVisita,
  DatosModalObservaciones,
} from '../analisis-tiendas.model';
import { ModalObservacionesService } from '../modal-observaciones/modal-obsevaciones.service';

@Component({
  selector: 'app-analisis-tiendas-container',
  standalone: true,
  imports: [CommonModule, SharedModule],
  providers: [AnalisisTiendasService, ModalObservacionesService],
  templateUrl: './analisis-tiendas-container.component.html',
  styleUrl: './analisis-tiendas-container.component.scss',
})
export class AnalisisTiendasContainerComponent {
  service = inject(AnalisisTiendasService);
  alerta = inject(AlertaService);
  modalObservacionesService = inject(ModalObservacionesService);
  analisisVisitas: AnalisisVisita[] = [];

  get tieneAnalisisVisitas() {
    return this.analisisVisitas.length > 0;
  }

  constructor() {
    this.obtenerAnalisisVisitas();
  }

  obtenerHoraComoFecha(hora: string) {
    return Utilidades.obtenerHoraComoFecha(hora);
  }

  obtenerTamanioArchivo(tamanio: number | undefined) {
    return Utilidades.obtenerTamanioArchivo(tamanio as number);
  }

  abrirModalObservacion(idVisita: number, observaciones: string | undefined) {
    const datosModal: DatosModalObservaciones = {
      idVisita,
      observaciones,
    };
    this.modalObservacionesService
      .abrirModal(datosModal)
      .pipe(take(1))
      .subscribe((huboCambios) => {
        if (huboCambios) this.obtenerAnalisisVisitas();
      });
  }

  obtenerAnalisisVisitas() {
    this.service
      .obtenerAnalisisVisitas()
      .pipe(take(1))
      .subscribe({
        next: (analisisVisitas) => {
          this.analisisVisitas = [...analisisVisitas];
        },
      });
  }

  descargarArchivo(archivo: Archivo) {
    Utilidades.descargarArchivoDesdeUrl(archivo);
  }
}
