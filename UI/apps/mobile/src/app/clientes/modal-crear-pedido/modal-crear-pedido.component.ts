import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { SharedModule } from '@storeflow/design-system';
import { ClientesStore } from '../state';
import { RegistroPedido } from '../../app.model';

@Component({
  selector: 'app-modal-crear-pedido',
  standalone: true,
  imports: [SharedModule, CommonModule],
  templateUrl: './modal-crear-pedido.component.html',
  styleUrl: './modal-crear-pedido.component.scss',
})
export class ModalCrearPedidoComponent {
  store = inject(ClientesStore);
  dialogRef = inject(MatDialogRef<ModalCrearPedidoComponent>);

  crearPedido() {
    const productosParaCrearPedido: RegistroPedido[] = this.store
      .productosSeleccionados()
      .map((producto) => ({
        codigo: producto.codigo,
        cantidad: producto.cantidad,
        precio: producto.precio,
      }));
    this.store.crearPedido(productosParaCrearPedido);
    this.dialogRef.close();
  }
}
