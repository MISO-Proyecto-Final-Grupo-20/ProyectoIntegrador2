import { Pedido, Producto, ProductoSeleccionado } from '../app.model';

export interface VendedoresState {
  clientes: Cliente[];
  filtroCliente: string;
  clienteSeleccionado: Cliente | null;
  productos: Producto[];
  productosSeleccionados: ProductoSeleccionado[];
  filtroProducto: string;
  pedidosPendientes: Pedido[];
  archivoSeleccionado: File | null;
  visitasRegistradas: Visita[];
  rutasAsignadas: RutaAsignada[];
  filtroRutasAsignadas: Date | null;
}

export interface Cliente {
  id: number;
  nombre: string;
  direccion: string;
}

export interface RegistrarVisita {
  fecha: Date;
  hora: string;
  archivo: File;
}

interface Archivo {
  nombre: string;
  tamanio: number;
}

export interface Visita {
  id: number;
  fecha: Date;
  hora: string;
  archivo: Archivo;
}

export interface RutaAsignada {
  cliente: string;
  direccion: string;
  fecha: Date;
}
