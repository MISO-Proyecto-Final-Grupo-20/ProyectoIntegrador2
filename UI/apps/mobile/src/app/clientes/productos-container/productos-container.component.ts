import { Component, inject } from '@angular/core';
import { ProductosComponent } from '../../shared/productos/productos.component';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ModalCrearPedidoService } from '../modal-crear-pedido/modal-crear-pedido.service';
import { ClientesStore } from '../state';
import { Producto } from '../../app.model';

@Component({
  selector: 'app-productos-container',
  standalone: true,
  imports: [ProductosComponent],
  providers: [ModalCrearPedidoService, ModalAgregarProductoService],
  template: `<app-productos
    [cantidadProductosSeleccionados]="cantidadProductosSeleccionados"
    (filtarProducto)="filtarProducto($event)"
    (seleccionarProducto)="seleccionarProducto($event)"
    (abrirModalCrearPedido)="abrirModalCrearPedido()"
    [productos]="store.productosFiltradosConSeleccion()"
  ></app-productos>`,
})
export class ProductosContainerComponent {
  modalAgregarProductoService = inject(ModalAgregarProductoService);
  modalCrearPedidoService = inject(ModalCrearPedidoService);
  store = inject(ClientesStore);

  get cantidadProductosSeleccionados(): number {
    return this.store.productosSeleccionados().length;
  }

  constructor() {
    this.store.asignarFiltroProductos('');
  }

  filtarProducto(valor: string) {
    this.store.asignarFiltroProductos(valor);
  }

  seleccionarProducto(producto: Producto) {
    if (producto.seleccionado) {
      this.store.seleccionarProducto({
        ...producto,
        cantidad: 0,
        seleccionado: false,
      });
    } else {
      this.modalAgregarProductoService.abrirModal(producto);
    }
  }

  abrirModalCrearPedido() {
    this.modalCrearPedidoService.abrirModal();
  }
}
