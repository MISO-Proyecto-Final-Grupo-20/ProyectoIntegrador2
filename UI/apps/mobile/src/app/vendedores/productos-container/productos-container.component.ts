import { Component, inject } from '@angular/core';
import { ProductosComponent } from '../../shared/productos/productos.component';
import { VendedoresStore } from '../state';
import { Producto } from '../../app.model';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ModalCrearPedidoService } from '../modal-crear-pedido/modal-crear-pedido.service';

@Component({
  selector: 'app-productos-container',
  standalone: true,
  imports: [ProductosComponent],
  template: `<app-productos
    [productos]="store.productosFiltradosConSeleccion()"
    [cantidadProductosSeleccionados]="cantidadProductosSeleccionados"
    (filtarProducto)="filtarProducto($event)"
    (seleccionarProducto)="seleccionarProducto($event)"
    (abrirModalCrearPedido)="abrirModalCrearPedido()"
  ></app-productos>`,
  providers: [ModalAgregarProductoService, ModalCrearPedidoService],
})
export class ProductosContainerComponent {
  modalAgregarProductoService = inject(ModalAgregarProductoService);
  modalCrearPedidoService = inject(ModalCrearPedidoService);
  store = inject(VendedoresStore);

  get cantidadProductosSeleccionados(): number {
    return this.store.productosSeleccionados().length;
  }

  constructor() {
    this.store.asignarFiltroProductos('');
    this.store.obtenerProductos();
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
