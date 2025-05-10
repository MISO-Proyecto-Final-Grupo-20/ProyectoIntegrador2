import { inject, Injectable } from '@angular/core';
import { ModalObservacionesComponent } from './modal-observaciones.component';
import { MatDialog } from '@angular/material/dialog';

@Injectable()
export class ModalObservacionesService {
  dialog = inject(MatDialog);

  abrirModal(idVisita: number) {
    this.dialog.open(ModalObservacionesComponent, {
      data: idVisita,
      width: '734px',
    });
  }
}
