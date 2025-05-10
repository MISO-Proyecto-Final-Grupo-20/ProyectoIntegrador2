import { patchState, withMethods } from '@ngrx/signals';
import { SignalsOf } from '@storeflow/design-system';
import { Cliente, VendedoresState } from '../vendedores.model';

export const actionsStore = withMethods(
  (state: SignalsOf<Partial<VendedoresState>>) => ({
    asignarFiltroClientes: (filtroCliente: string) =>
      patchState(state, { filtroCliente }),
    seleccionarCliente: (clienteSeleccionado: Cliente) =>
      patchState(state, { clienteSeleccionado }),
    asignarFiltroProductos: (filtroProducto: string) =>
      patchState(state, { filtroProducto }),
    seleccionarArchivo: (archivoSeleccionado: File | null) =>
      patchState(state, { archivoSeleccionado }),
    asignarFiltroRutasAsignadas: (filtroRutasAsignadas: Date | null) =>
      patchState(state, { filtroRutasAsignadas }),
  })
);
