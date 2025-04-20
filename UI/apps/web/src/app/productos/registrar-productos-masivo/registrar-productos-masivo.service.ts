import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RegistrarProductosMasivoComponent } from './registrar-productos-masivo.component';

@Injectable()
export class RegistrarProductosMasivoService {
  dialog = inject(MatDialog);

  abrirModal() {
    this.dialog.open(RegistrarProductosMasivoComponent, {
      width: '1067px',
      height: '593px',
      maxHeight: '90vh',
    });
  }
}
