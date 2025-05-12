import { inject, Injectable } from '@angular/core';
import { ModalObservacionesComponent } from './modal-observaciones.component';
import { MatDialog } from '@angular/material/dialog';
import { DatosModalObservaciones } from '../analisis-tiendas.model';
import { Observable } from 'rxjs';

@Injectable()
export class ModalObservacionesService {
  dialog = inject(MatDialog);

  abrirModal(data: DatosModalObservaciones): Observable<boolean | void> {
    const dialogRef = this.dialog.open(ModalObservacionesComponent, {
      data,
      width: '734px',
    });
    return dialogRef.afterClosed();
  }
}
