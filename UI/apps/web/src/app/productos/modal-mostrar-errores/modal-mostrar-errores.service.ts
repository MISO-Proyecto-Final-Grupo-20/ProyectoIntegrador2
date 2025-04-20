import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalMostrarErroresComponent } from './modal-mostrar-errores.component';

@Injectable()
export class ModalMostrarErroresService {
  dialog = inject(MatDialog);

  abrirModal(errores: string[]) {
    this.dialog.open(ModalMostrarErroresComponent, {
      width: '600px',
      maxHeight: '80vh',
      data: errores,
    });
  }
}
