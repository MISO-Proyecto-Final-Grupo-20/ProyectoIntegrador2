/* eslint-disable @typescript-eslint/no-explicit-any */
import { Sesion, TipoCategoria } from '../modelos/generales.model';

export class Utilidades {
  static obtenerSesion(): Sesion {
    //quitar
    return {
      categoria: TipoCategoria.Cliente,
      nombreUsuario: 'Camilo Barreto',
    } as Sesion;
    // return (window as any).sesion as Sesion;
  }

  static obtenerTamanioArchivo(size: number): string {
    const bytes = size ?? 0;

    if (bytes < 1024) {
      return `${bytes} B`;
    } else if (bytes < 1024 * 1024) {
      const kb = bytes / 1024;
      return `${Math.round(kb)} KB`;
    } else if (bytes < 1024 * 1024 * 1024) {
      const mb = bytes / (1024 * 1024);
      return `${Math.round(mb)} MB`;
    } else {
      const gb = bytes / (1024 * 1024 * 1024);
      return `${Math.round(gb)} GB`;
    }
  }
}
