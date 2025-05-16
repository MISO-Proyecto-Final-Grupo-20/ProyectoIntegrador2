import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalSeleccionarProductosComponent } from './modal-seleccionar-productos.component';

@Injectable()
export class ModalSeleccionarProductosService {
  private dialog = inject(MatDialog);
  abrirModal() {
    this.dialog.open(ModalSeleccionarProductosComponent, {
      width: '60%',
    });
  }
}
