import { OpcionesLista } from '@storeflow/design-system';
import { Vendedor } from '../app-model';

export const periodosTiempoMock: OpcionesLista = [
  {
    id: 1,
    descripcion: 'Mensual',
  },
  {
    id: 2,
    descripcion: 'Anual',
  },
  {
    id: 3,
    descripcion: 'Semestral',
  },
  {
    id: 4,
    descripcion: 'Trimestral',
  },
];

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
