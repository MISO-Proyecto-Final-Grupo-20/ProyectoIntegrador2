import { Component, effect, inject } from '@angular/core';
import { SharedModule } from '@storeflow/design-system';
import { ComprasBodegaStore } from '../state';
import { CommonModule } from '@angular/common';
import { ProductoSeleccionado } from '../compras-bodega.model';
import { Producto } from '../../app-model';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-modal-seleccionar-productos',
  standalone: true,
  imports: [SharedModule, CommonModule],
  templateUrl: './modal-seleccionar-productos.component.html',
  styleUrl: './modal-seleccionar-productos.component.scss',
})
export class ModalSeleccionarProductosComponent {
  store = inject(ComprasBodegaStore);
  private dialogRef = inject(MatDialogRef<ModalSeleccionarProductosComponent>);
  productosConCantidad = new Map<number, ProductoSeleccionado>();
  productosSeleccionados = new Set<Producto>();
  cantidadProductosInicial = 10;

  get totalValorProductosSeleccionados(): number {
    let total = 0;

    this.productosSeleccionados.forEach((producto) => {
      const cantidad = this.obtenerCantidad(producto.id);
      total += cantidad * producto.precio;
    });

    return total;
  }

  get desabilitarBoton(): boolean {
    return this.productosSeleccionados.size === 0;
  }

  constructor() {
    effect(() => {
      this.productosSeleccionados.clear();
      this.productosConCantidad.clear();
      this.store.productosSeleccionados().forEach((productoSeleccionado) => {
        this.productosSeleccionados.add(productoSeleccionado);
        this.productosConCantidad.set(
          productoSeleccionado.id,
          productoSeleccionado
        );
      });
    });
  }

  sumarProducto(producto: Producto) {
    const cantidadActual = this.obtenerCantidad(producto.id);
    this.productosConCantidad.set(producto.id, {
      ...producto,
      cantidad: cantidadActual + 1,
    });
  }

  restarProducto(producto: Producto) {
    const cantidadActual = this.obtenerCantidad(producto.id);
    if (cantidadActual > 1) {
      this.productosConCantidad.set(producto.id, {
        ...producto,
        cantidad: cantidadActual - 1,
      });
    }
  }

  obtenerCantidad(id: number): number {
    const item = this.productosConCantidad.get(id);
    return item?.cantidad ?? this.cantidadProductosInicial;
  }

  agregarProductos() {
    this.store.seleccionarProductos(
      Array.from(this.productosSeleccionados).map((producto) => ({
        ...producto,
        cantidad: this.obtenerCantidad(producto.id),
      }))
    );
    this.dialogRef.close();
  }

  seleccionarProducto(producto: Producto, event: MatCheckboxChange) {
    if (event.checked) {
      this.productosSeleccionados.add(producto);
      return;
    }
    this.eliminarProductoPorId(producto);
  }

  eliminarProductoPorId(producto: Producto) {
    const productoAEliminar = Array.from(this.productosSeleccionados).find(
      (p) => p.id === producto.id
    );
    if (productoAEliminar) {
      this.productosSeleccionados.delete(productoAEliminar);
    }
  }

  esProductoSeleccionado(producto: Producto): boolean {
    return Array.from(this.productosSeleccionados).some(
      (p) => p.id === producto.id
    );
  }
}
