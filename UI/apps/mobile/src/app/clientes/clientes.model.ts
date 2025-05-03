import { Pedido } from '../app.model';

export interface Producto {
  imagen: string;
  nombre: string;
  codigo: string;
  precio: number;
  seleccionado?: boolean;
}

export interface ProductoSeleccionado extends Producto {
  cantidad: number;
}

export interface ClientesState {
  productos: Producto[];
  filtroProducto: string | null;
  productosSeleccionados: ProductoSeleccionado[];
  pedidos: Pedido[];
  filtroPedido: string | null;
}

export interface RegistroPedido {
  codigo: string;
  cantidad: number;
  precio: number;
}

export interface TabsCrearPedido {
  titulo: string;
  ruta: string;
}
