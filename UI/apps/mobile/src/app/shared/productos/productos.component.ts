import { Component, input, output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@storeflow/design-system';
import { Producto } from '../../app.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule, CommonModule],
  templateUrl: './productos.component.html',
  styleUrl: './productos.component.scss',
})
export class ProductosComponent {
  productos = input<Producto[]>();
  cantidadProductosSeleccionados = input<number>();
  controlBuscar = new FormControl('');
  seleccionarProducto = output<Producto>();
  abrirModalCrearPedido = output<void>();
  filtarProducto = output<string>();

  constructor() {
    this.controlBuscar.valueChanges.subscribe((valor) => {
      this.filtarProducto.emit(valor ?? '');
    });
  }

  emitirSeleccionarProducto(producto: Producto) {
    this.seleccionarProducto.emit(producto);
  }

  emitirAbrirModalCrearPedido() {
    this.abrirModalCrearPedido.emit();
  }
}
