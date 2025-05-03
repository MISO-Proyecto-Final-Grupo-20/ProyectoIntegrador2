import { Producto, Vendedor } from './app-model';

export const vendedoresMock: Vendedor[] = [
  { id: 1, nombre: 'Camilo Barretor', correo: 'camilo@barreto.co' },
  { id: 11, nombre: 'Camilo Barretor', correo: 'camilo@barreto.co' },
  { id: 111, nombre: 'Camilo Barretor', correo: 'camilo@barreto.co' },
  { id: 2, nombre: 'Augusto Romero', correo: 'augusto@romero.co' },
  { id: 222, nombre: 'Augusto Romero', correo: 'augusto@romero.co' },
  { id: 22, nombre: 'Augusto Romero', correo: 'augusto@romero.co' },
  { id: 3, nombre: 'Augusto Marinez', correo: 'augusto@marinez.co' },
  { id: 33, nombre: 'Augusto Marinez', correo: 'augusto@marinez.co' },
  { id: 333, nombre: 'Augusto Marinez', correo: 'augusto@marinez.co' },
];

export const productosMock: Producto[] = [
  {
    id: 1,
    nombre: 'Paca de leche x12 unidades',
    fabricanteAsociado: { id: 2, nombre: 'Alquería S.A.' },
    codigo: 'A7X9B3Q5LZ82MND4VYKCJ6T1W0GFRP',
    imagen:
      'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
    precio: 1000,
  },
  {
    id: 2,
    nombre: 'Chocolatinas JET x40 unidades',
    fabricanteAsociado: { id: 1, nombre: 'Grupo Nutresa S.A.' },
    codigo: 'MND4VYKCJ6T1W0GFRP',
    imagen:
      'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
    precio: 1000,
  },
  {
    id: 3,
    nombre: 'Galletas Festival x 10 unidades',
    fabricanteAsociado: { id: 1, nombre: 'Grupo Nutresa S.A.' },
    codigo: 'A7X9B3Q5LZ82MND4VY',
    imagen:
      'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
    precio: 1000,
  },
];
