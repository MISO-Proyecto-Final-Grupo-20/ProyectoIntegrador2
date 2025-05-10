import { signalStore, withState } from '@ngrx/signals';
import { ClientesState } from '../clientes.model';
import { actionsStore } from './actions';
import { effectsStore } from './effects';
import { selectorsStore } from './selector';

const initialState: ClientesState = {
  productos: [],
  filtroProducto: '',
  productosSeleccionados: [],
  pedidos: [],
  filtroPedido: '',
  entregasProgramadas: [],
  filtroEntrega: '',
};

export const ClientesStore = signalStore(
  withState(initialState),
  effectsStore,
  actionsStore,
  selectorsStore
);
