export interface ListadoFabricantes {
  id: number;
  nombre: string;
}

export interface Producto {
  nombre: string;
  fabricanteAsociado?: number;
  codigo: number;
  precio: number;
  imagen: string;
}
