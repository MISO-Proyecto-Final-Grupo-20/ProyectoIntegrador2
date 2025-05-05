import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalCrearPedidoComponent } from './modal-crear-pedido.component';

@Injectable()
export class ModalCrearPedidoService {
  dialog = inject(MatDialog);

  abrirModal() {
    this.dialog.open(ModalCrearPedidoComponent, {
      width: '90%',
      maxHeight: '90vh',
    });
  }
}
