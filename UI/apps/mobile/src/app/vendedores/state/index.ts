import { signalStore, withState } from '@ngrx/signals';
import { actionsStore } from './actions';
import { effectsStore } from './effects';
import { selectorsStore } from './selector';
import { VendedoresState } from '../vendedores.model';

const initialState: VendedoresState = {
  clientes: [],
  filtroCliente: '',
  clienteSeleccionado: null,
  productos: [],
  productosSeleccionados: [],
  filtroProducto: '',
  pedidosPendientes: [],
};

export const VendedoresStore = signalStore(
  withState(initialState),
  effectsStore,
  actionsStore,
  selectorsStore
);
