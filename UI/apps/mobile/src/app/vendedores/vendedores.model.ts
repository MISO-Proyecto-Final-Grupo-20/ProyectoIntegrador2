import { Pedido, Producto, ProductoSeleccionado } from '../app.model';

export interface VendedoresState {
  clientes: Cliente[];
  filtroCliente: string;
  clienteSeleccionado: Cliente | null;
  productos: Producto[];
  productosSeleccionados: ProductoSeleccionado[];
  filtroProducto: string;
  pedidosPendientes: Pedido[];
}

export interface Cliente {
  id: number;
  nombre: string;
  direccion: string;
}
