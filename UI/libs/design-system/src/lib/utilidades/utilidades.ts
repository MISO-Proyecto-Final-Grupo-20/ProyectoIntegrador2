/* eslint-disable @typescript-eslint/no-explicit-any */
import { Sesion, TipoCategoria } from '../modelos/generales.model';

export class Utilidades {
  static obtenerSesion(): Sesion {
    //quitar
    return {
      categoria: TipoCategoria.Vendedor,
      nombreUsuario: 'Camilo Barreto',
    } as Sesion;
    // return (window as any).sesion as Sesion;
  }
}
