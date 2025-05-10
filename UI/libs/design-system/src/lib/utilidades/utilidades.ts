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

  static obtenerHoraComoFecha(hora: string): Date {
    const [horas, minutos] = hora.split(':');
    const fecha = new Date();
    fecha.setHours(Number(horas), Number(minutos), 0, 0);
    return fecha;
  }

  static descargarArchivo(archivo: Blob, nombreArchivo: string): void {
    const url = URL.createObjectURL(archivo);
    const extension = Utilidades.obtenerExtensionPorMIME(archivo.type);
    const a = document.createElement('a');
    a.href = url;
    a.download = `${nombreArchivo}${extension}`;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  }

  static obtenerExtensionPorMIME(mimeType: string): string {
    const extensionMap: { [key: string]: string } = {
      'video/mp4': '.mp4',
      'video/webm': '.webm',
      'video/ogg': '.ogg',
      'video/avi': '.avi',
      'video/mkv': '.mkv',
    };
    return extensionMap[mimeType] || '';
  }
}
