export interface ListadoFabricantes {
  id: number;
  nombre: string;
}

export interface Producto {
  nombre: string;
  fabricanteAsociado: ListadoFabricantes;
  codigo: string;
  precio: number;
  imagen: string;
}

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
