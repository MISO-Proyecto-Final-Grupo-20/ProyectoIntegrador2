import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@storeflow/design-system';
import { Producto } from '../clientes.model';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ModalCrearPedidoService } from '../modal-crear-pedido/modal-crear-pedido.service';
import { ClientesStore } from '../state';

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule, CommonModule],
  providers: [ModalAgregarProductoService, ModalCrearPedidoService],
  templateUrl: './productos.component.html',
  styleUrl: './productos.component.scss',
})
export class ProductosComponent {
  modalAgregarProductoService = inject(ModalAgregarProductoService);
  modalCrearPedidoService = inject(ModalCrearPedidoService);
  store = inject(ClientesStore);
  controlBuscar = new FormControl('');

  get cantidadProductosSeleccionados(): number {
    return this.store.productosSeleccionados().length;
  }

  constructor() {
    this.store.asignarFiltroProductos('');
    this.controlBuscar.valueChanges.subscribe((valor) => {
      this.store.asignarFiltroProductos(valor);
    });
  }

  seleccionarProducto(producto: Producto) {
    if (producto.seleccionado) {
      this.store.seleccionarProducto({ ...producto, cantidad: 0 });
    } else {
      this.modalAgregarProductoService.abrirModal(producto);
    }
  }

  abrirModalCrearPedido() {
    this.modalCrearPedidoService.abrirModal();
  }
}
