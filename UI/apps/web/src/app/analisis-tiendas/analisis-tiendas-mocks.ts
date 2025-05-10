import { AnalisisVisita } from './analisis-tiendas.model';

export const AnalisisTiendasMocks: AnalisisVisita[] = [
  {
    id: 189,
    fecha: new Date(2025, 3, 23),
    hora: '15:37',
    cliente: 'Surtimax',
    archivo: {
      nombre: 'Pedido semana 15',
      tamanio: 123456,
    },
  },
  {
    id: 188,
    fecha: new Date(2025, 3, 23),
    hora: '15:37',
    cliente: 'Alqueria',
    archivo: {
      nombre: 'Pedido semana 15',
      tamanio: 123456,
    },
    observaciones: ['No se pudo realizar la visita'],
  },
  {
    id: 187,
    fecha: new Date(2025, 3, 23),
    hora: '15:37',
    cliente: 'Bogota',
    archivo: {
      nombre: 'Pedido semana 15',
      tamanio: 123456,
    },
  },
];
