import { Component, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@storeflow/design-system';
import { Producto } from '../../app.model';

@Component({
  selector: 'app-agregar-producto',
  standalone: true,
  imports: [SharedModule, CommonModule],
  templateUrl: './agregar-producto.component.html',
  styleUrl: './agregar-producto.component.scss',
})
export class AgregarProductoComponent {
  agregarProducto = output<number>();
  producto = input<Producto>({} as Producto);
  cantidadProductos = signal(10);
  sumarProducto() {
    this.cantidadProductos.update((cantidad) => cantidad + 1);
  }

  restarProducto() {
    this.cantidadProductos.update((cantidad) =>
      cantidad > 1 ? cantidad - 1 : cantidad
    );
  }

  emitirAgregarProducto() {
    this.agregarProducto.emit(this.cantidadProductos());
  }
}
