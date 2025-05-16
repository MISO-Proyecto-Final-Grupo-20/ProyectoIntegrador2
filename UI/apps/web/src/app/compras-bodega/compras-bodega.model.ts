import { ListadoFabricantes, Producto } from '../app-model';
import { OpcionesLista } from '@storeflow/design-system';

export interface ComprasBodegaState {
  listadoFabricantes: ListadoFabricantes[];
  listadoBodegas: OpcionesLista;
  productosSeleccionados: ProductoSeleccionado[];
  productos: Producto[];
}

export interface ProductoSeleccionado extends Producto {
  cantidad: number;
}

export interface FormularioRegistroBodega {
  bodega: number;
  fabricante: number;
}

export interface RegistroCompraBodega extends FormularioRegistroBodega {
  productos: ProductoSeleccionadoRegistro[];
}

export interface ProductoSeleccionadoRegistro {
  id: number;
  cantidad: number;
}
