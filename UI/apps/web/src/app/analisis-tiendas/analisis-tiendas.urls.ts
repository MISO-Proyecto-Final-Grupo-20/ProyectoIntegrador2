import { environment } from '../../environments/environment';

export const AnalisisTiendasUrls = {
  obtenerAnalisisVisitas: `${environment.apiUrl}/ventas/visitas/analisis`,
  guardarObsevacionesAnalisisVisitas: `${environment.apiUrl}/ventas/visitas/analisis/[idvisita]/observaciones`,
  descargarArchivo: `${environment.apiUrl}/ventas/visitas/analisis/[idvisita]/archivo`,
};
