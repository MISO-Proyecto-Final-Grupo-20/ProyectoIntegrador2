import { signalStore, withState } from '@ngrx/signals';
import { actionsStore } from './actions';
import { effectsStore } from './effects';
import { selectorsStore } from './selector';
import { ComprasBodegaState } from '../compras-bodega.model';

const initialState: ComprasBodegaState = {
  listadoBodegas: [],
  listadoFabricantes: [],
  productos: [],
  productosSeleccionados: [],
};

export const ComprasBodegaStore = signalStore(
  withState(initialState),
  effectsStore,
  actionsStore,
  selectorsStore
);
