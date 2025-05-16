import { patchState, withMethods } from '@ngrx/signals';
import { SignalsOf } from '@storeflow/design-system';
import {
  ComprasBodegaState,
  ProductoSeleccionado,
} from '../compras-bodega.model';

export const actionsStore = withMethods(
  (state: SignalsOf<Partial<ComprasBodegaState>>) => ({
    seleccionarProductos: (productosSeleccionados: ProductoSeleccionado[]) =>
      patchState(state, {
        productosSeleccionados: [...productosSeleccionados],
      }),
  })
);
