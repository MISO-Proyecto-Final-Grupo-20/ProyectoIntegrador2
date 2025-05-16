import { computed } from '@angular/core';
import { StateSignals, withComputed } from '@ngrx/signals';
import {
  ComprasBodegaState,
  ProductoSeleccionado,
  ProductoSeleccionadoRegistro,
} from '../compras-bodega.model';

export const selectorsStore = withComputed((store) => {
  const { productosSeleccionados } = store as StateSignals<ComprasBodegaState>;

  return {
    cantidadProductosSeleccionados: computed(
      () => productosSeleccionados().length
    ),
    productosSeleccionadosRegistro: computed(() =>
      obtenerProductosSeleccionados(productosSeleccionados())
    ),
  };
});

function obtenerProductosSeleccionados(
  productosSeleccionados: ProductoSeleccionado[]
): ProductoSeleccionadoRegistro[] {
  return productosSeleccionados.map((producto) => ({
    id: producto.id,
    cantidad: producto.cantidad,
  }));
}
