import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  AlertaService,
  SharedModule,
  TipoAlerta,
  Utilidades,
} from '@storeflow/design-system';
import { AnalisisTiendasService } from '../analisis-tiendas.service';
import { take } from 'rxjs';
import { AnalisisVisita } from '../analisis-tiendas.model';
import { ModalObservacionesService } from '../modal-observaciones/modal-obsevaciones.service';
import { MensajesAnalisisTienda } from '../analisis-tiendas.constantes';

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

  descargarArchivo(idVisita: number) {
    const nombreArchivo = `Visita_${idVisita}.mp4`;
    this.service
      .descargarArchivo(idVisita)
      .pipe(take(1))
      .subscribe({
        next: (archivo) => {
          Utilidades.descargarArchivo(archivo, nombreArchivo);
          this.alerta.abrirAlerta({
            tipo: TipoAlerta.Success,
            descricion: MensajesAnalisisTienda.descargaArchivoExitoso,
          });
        },
      });
  }
}
