/* eslint-disable @typescript-eslint/no-explicit-any */
import { Sesion } from '../modelos/generales.model';

export class Utilidades {
  static obtenerSesion(): Sesion {
    return (window as any).sesion as Sesion;
  }
}
