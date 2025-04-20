import { Signal } from '@angular/core';
import { WritableStateSource } from '@ngrx/signals';

export type OpcionesLista = OpcionLista[];

export enum TipoCategoria {
  Cliente = 'cliente',
  Vendedor = 'vendedor',
}

export interface Sesion {
  nombreUsuario: string;
  email: string;
  categoria: TipoCategoria;
}

export interface DatosIngreso {
  correo: string;
  contrasena: string;
}

export interface OpcionLista {
  id: number;
  descripcion: string;
}

export type SignalsOf<T extends Record<string, any>> = {
  [K in keyof T]: Signal<T[K]>;
} & WritableStateSource<T>;
