import { CommonModule } from '@angular/common';
import { Component, inject, Inject, signal } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SharedModule } from '@storeflow/design-system';
import { Producto } from '../clientes.model';
import { ClientesStore } from '../state';

@Component({
  selector: 'app-modal-agregar-producto',
  standalone: true,
  imports: [SharedModule, CommonModule],
  templateUrl: './modal-agregar-producto.component.html',
  styleUrl: './modal-agregar-producto.component.scss',
})
export class ModalAgregarProductoComponent {
  store = inject(ClientesStore);
  cantidadProductos = signal(10);

  constructor(@Inject(MAT_DIALOG_DATA) public producto: Producto) {}

  sumarProducto() {
    this.cantidadProductos.update((cantidad) => cantidad + 1);
  }

  restarProducto() {
    this.cantidadProductos.update((cantidad) =>
      cantidad > 1 ? cantidad - 1 : cantidad
    );
  }

  agregarProducto() {
    this.store.validarInventarioProducto({
      ...this.producto,
      seleccionado: true,
      cantidad: this.cantidadProductos(),
    });
  }
}
