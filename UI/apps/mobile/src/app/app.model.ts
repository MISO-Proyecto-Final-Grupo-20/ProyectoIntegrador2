import { EstadoPedido } from './app.enum';

export interface ConfiguracionMenu {
  titulo: string;
  imagen: {
    width: number;
    height: number;
    src: string;
  };
  ruta: string;
}

export interface Pedido {
  numero: number;
  fechaRegistro: Date;
  estado: EstadoPedido;
  total: number;
  fechaEntrega: Date;
  lugarEntrega: string;
  productos: ProductoPedido[];
}

export interface ProductoPedido {
  id: number;
  imagen: string;
  nombre: string;
  codigo: string;
  precio: number;
  cantidad: number;
}

export interface ConfiguracionEstadoPedido {
  descripcion: string;
  color: string;
}
