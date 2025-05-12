import { environment } from '../../environments/environment';

export const AnalisisTiendasUrls = {
  obtenerAnalisisVisitas: `${environment.apiUrl}/ventas/visitas/analisis`,
  guardarObsevacionesAnalisisVisitas: `${environment.apiUrl}/ventas/visitas/analisis/[idVisita]/observaciones`,
  descargarArchivo: `${environment.apiUrl}/ventas/visitas/analisis/[idVisita]/archivo`,
};
