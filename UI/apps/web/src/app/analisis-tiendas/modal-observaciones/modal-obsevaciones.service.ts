import { inject, Injectable } from '@angular/core';
import { ModalObservacionesComponent } from './modal-observaciones.component';
import { MatDialog } from '@angular/material/dialog';
import { DatosModalObservaciones } from '../analisis-tiendas.model';

@Injectable()
export class ModalObservacionesService {
  dialog = inject(MatDialog);

  abrirModal(data: DatosModalObservaciones) {
    this.dialog.open(ModalObservacionesComponent, {
      data,
      width: '734px',
    });
  }
}
