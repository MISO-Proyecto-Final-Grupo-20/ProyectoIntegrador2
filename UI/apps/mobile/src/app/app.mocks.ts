import { EstadoPedido } from './app.enum';
import { Pedido, Producto, ProductoPedido } from './app.model';

export const productos: ProductoPedido[] = [
  {
    id: 1,
    imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
    nombre: 'Leche ',
    precio: 20000,
    codigo: '123456789',
    cantidad: 10,
  },
  {
    id: 2,
    imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
    nombre: 'Jugo de naranja',
    precio: 10000,
    codigo: '987654321',
    cantidad: 10,
  },

  {
    id: 1,
    imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
    nombre: 'Leche ',
    precio: 20000,
    codigo: '123456789',
    cantidad: 10,
  },
  {
    id: 2,
    imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
    nombre: 'Jugo de naranja',
    precio: 10000,
    codigo: '987654321',
    cantidad: 10,
  },
  {
    id: 1,
    imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
    nombre: 'Leche ',
    precio: 20000,
    codigo: '123456789',
    cantidad: 10,
  },
  {
    id: 2,
    imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
    nombre: 'Jugo de naranja',
    precio: 10000,
    codigo: '987654321',
    cantidad: 10,
  },
  {
    id: 1,
    imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
    nombre: 'Leche ',
    precio: 20000,
    codigo: '123456789',
    cantidad: 10,
  },
  {
    id: 2,
    imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
    nombre: 'Jugo de naranja',
    precio: 10000,
    codigo: '987654321',
    cantidad: 10,
  },
  {
    id: 1,
    imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
    nombre: 'Leche ',
    precio: 20000,
    codigo: '123456789',
    cantidad: 10,
  },
  {
    id: 2,
    imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
    nombre: 'Jugo de naranja',
    precio: 10000,
    codigo: '987654321',
    cantidad: 10,
  },
];

export const mocksPedidos: Pedido[] = [
  {
    numero: 578,
    estado: EstadoPedido.pendiente,
    fechaRegistro: new Date(2025, 3, 23),
    fechaEntrega: new Date(2025, 3, 23),
    lugarEntrega: 'Cr 238#47- 889',
    total: 400000,
    productos,
  },
  {
    numero: 578,
    estado: EstadoPedido.pendiente,
    fechaRegistro: new Date(2025, 3, 23),
    fechaEntrega: new Date(2025, 3, 23),
    lugarEntrega: 'Cr 238#47- 889',
    total: 400000,
    productos,
  },
  {
    numero: 578,
    estado: EstadoPedido.enCamino,
    fechaRegistro: new Date(2025, 3, 23),
    fechaEntrega: new Date(2025, 3, 23),
    lugarEntrega: 'Cr 238#47- 889',
    total: 400000,
    productos,
  },
  {
    numero: 555,
    estado: EstadoPedido.entregado,
    fechaRegistro: new Date(2025, 3, 23),
    fechaEntrega: new Date(2025, 3, 23),
    lugarEntrega: 'Cr 238#47- 889',
    total: 500000,
    productos,
  },
];

export const mockProductos: Producto[] = [
  {
    imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
    nombre: 'Leche Leche  de almendra sin azucar 1L, Almendras,',
    precio: 20000,
    codigo: '123456789',
  },
  {
    imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
    nombre: 'Jugo de naranja',
    precio: 10000,
    codigo: '987654321',
  },
];
