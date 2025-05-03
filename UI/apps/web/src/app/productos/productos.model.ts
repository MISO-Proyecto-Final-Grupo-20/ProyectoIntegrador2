import { Producto } from '../app-model';

export interface RegistrarProducto {
  nombre: string;
  fabricanteAsociado: number;
  codigo: string;
  precio: number;
  imagen: string;
}

export interface ResultadoCargaMasiva {
  errores: string[];
  productos: Producto[];
}
