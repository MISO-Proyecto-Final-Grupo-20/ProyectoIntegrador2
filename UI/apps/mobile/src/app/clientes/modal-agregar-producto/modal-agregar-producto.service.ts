import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Producto } from '../clientes.model';
import { ModalAgregarProductoComponent } from './modal-agregar-producto.component';

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
