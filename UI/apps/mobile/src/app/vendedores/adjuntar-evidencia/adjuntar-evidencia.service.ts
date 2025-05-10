import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AdjuntarEvidenciaComponent } from './adjuntar-evidencia.component';

@Injectable()
export class AdjuntarEvidenciaService {
  dialog = inject(MatDialog);

  abrirModal() {
    this.dialog.open(AdjuntarEvidenciaComponent, {
      width: '314px',
    });
  }
}
