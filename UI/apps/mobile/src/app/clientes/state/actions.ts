import { patchState, withMethods } from '@ngrx/signals';
import { SignalsOf } from '@storeflow/design-system';
import { ClientesState } from '../clientes.model';

export const actionsStore = withMethods(
  (state: SignalsOf<Partial<ClientesState>>) => ({
    asignarFiltroProductos: (filtroProducto: string) =>
      patchState(state, { filtroProducto }),
    asignarFiltroPedidos: (filtroPedido: string) =>
      patchState(state, { filtroPedido }),
  })
);
