import {
  Pedido,
  Producto,
  ProductoPedido,
  ProductoSeleccionado,
} from '../app.model';

export interface ClientesState {
  productos: Producto[];
  filtroProducto: string;
  productosSeleccionados: ProductoSeleccionado[];
  pedidos: Pedido[];
  filtroPedido: string;
  entregasProgramadas: EntregaProgramada[];
  filtroEntrega: string;
}

export interface EntregaProgramada {
  id: number;
  numero: number;
  fechaEntrega: Date;
  lugarEntrega: string;
  productos: ProductoPedido[];
}
