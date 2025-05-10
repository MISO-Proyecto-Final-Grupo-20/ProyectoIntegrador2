/* eslint-disable @typescript-eslint/no-explicit-any */
import { Sesion, TipoCategoria } from '../modelos/generales.model';

export class Utilidades {
  static obtenerSesion(): Sesion {
    const sesion = (window as any).sesion;
    if (!sesion) return {} as Sesion;
    const claimsBase =
      'http://schemas.microsoft.com/ws/2008/06/identity/claims';
    const nombre = sesion['nombre'] ?? sesion['correo'];
    const categoria = sesion[`${claimsBase}/role`] as TipoCategoria;

    return {
      nombre,
      email: sesion['correo'],
      categoria: categoria,
    };
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
