import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalAgregarProductoComponent } from './modal-agregar-producto.component';
import { Producto } from '../../app.model';

@Injectable()
export class ModalAgregarProductoService {
  dialog = inject(MatDialog);

  abrirModal(producto: Producto) {
    this.dialog.open(ModalAgregarProductoComponent, {
      data: producto,
      width: '90%',
    });
  }

  cerrarModal() {
    this.dialog.closeAll();
  }
}
