export interface Vendedor {
  id: number;
  nombre: string;
  correo: string;
}

export interface ListadoFabricantes {
  id: number;
  nombre: string;
}

export interface Producto {
  id: number;
  nombre: string;
  fabricanteAsociado: ListadoFabricantes;
  codigo: string;
  precio: number;
  imagen: string;
}
