import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@storeflow/design-system';
import { ProductoPedido } from '../../app.model';

@Component({
  selector: 'app-listado-productos',
  standalone: true,
  imports: [CommonModule, SharedModule],
  template: `<div>
    @for (producto of productos(); track $index) {
      <mat-list class="bg-white overflow">
        <mat-list-item>
          <div class="row justify-content-between">
            <div class="row gap-8 align-items-center">
              <div
                class="tamanio-30 column align-items-center justify-content-center"
              >
                <img
                  class="max-tamanio-30"
                  [src]="producto.imagen"
                  alt="imagen producto"
                />
              </div>
              <div class="gap-2">
                <p class="mat-body-1 color-grey-800">
                  {{ producto.nombre }}
                </p>
                <p class="color-accent">{{ producto.precio | number }} COP</p>
              </div>
            </div>
            <h3 class="color-grey-800 min-width-27">
              {{ producto.cantidad }}
            </h3>
          </div>
        </mat-list-item>
      </mat-list>
    }
  </div>`,
  styleUrl: './listado-productos.component.scss',
})
export class ListadoProductosComponent {
  productos = input<ProductoPedido[]>([]);
}
