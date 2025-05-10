import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule, Utilidades } from '@storeflow/design-system';
import { AnalisisTiendasService } from '../analisis-tiendas.service';
import { take } from 'rxjs';
import { AnalisisVisita } from '../analisis-tiendas.model';
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
  modalObservacionesService = inject(ModalObservacionesService);
  analisisVisitas: AnalisisVisita[] = [];

  get tieneAnalisisVisitas() {
    return this.analisisVisitas.length > 0;
  }

  constructor() {
    this.service
      .obtenerAnalisisVisitas()
      .pipe(take(1))
      .subscribe({
        next: (analisisVisitas) => {
          this.analisisVisitas = [...analisisVisitas];
        },
      });
  }

  obtenerHoraComoFecha(hora: string) {
    return Utilidades.obtenerHoraComoFecha(hora);
  }

  obtenerTamanioArchivo(tamanio: number | undefined) {
    return Utilidades.obtenerTamanioArchivo(tamanio as number);
  }

  abrirModalObservacion(idVisita: number) {
    this.modalObservacionesService.abrirModal(idVisita);
  }
}
