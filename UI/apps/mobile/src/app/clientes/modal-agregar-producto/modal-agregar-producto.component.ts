import { Component, inject, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SharedModule } from '@storeflow/design-system';
import { ClientesStore } from '../state';
import { Producto } from '../../app.model';
import { AgregarProductoComponent } from '../../shared/agregar-producto/agregar-producto.component';

@Component({
  selector: 'app-modal-agregar-producto',
  standalone: true,
  imports: [SharedModule, AgregarProductoComponent],
  template: `<section>
    <div class="row justify-content-end pt-12 px-16">
      <mat-icon class="cursor-pointer color-grey-800" mat-dialog-close
        >close</mat-icon
      >
    </div>
    <app-agregar-producto
      [producto]="producto"
      (agregarProducto)="agregarProducto($event)"
    ></app-agregar-producto>
  </section> `,
})
export class ModalAgregarProductoComponent {
  store = inject(ClientesStore);

  constructor(@Inject(MAT_DIALOG_DATA) public producto: Producto) {}

  agregarProducto(cantidad: number) {
    this.store.validarInventarioProducto({
      ...this.producto,
      seleccionado: true,
      cantidad: cantidad,
    });
  }
}
