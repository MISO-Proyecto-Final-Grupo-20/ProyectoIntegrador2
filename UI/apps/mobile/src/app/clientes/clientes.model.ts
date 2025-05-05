import { Pedido, Producto, ProductoSeleccionado } from '../app.model';

export interface ClientesState {
  productos: Producto[];
  filtroProducto: string;
  productosSeleccionados: ProductoSeleccionado[];
  pedidos: Pedido[];
  filtroPedido: string;
}
