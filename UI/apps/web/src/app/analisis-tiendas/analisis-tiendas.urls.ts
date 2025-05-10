import { environment } from '../../environments/environment';

export const AnalisisTiendasUrls = {
  obtenerAnalisisVisitas: `${environment.apiUrl}/visitas/analisis`,
  guardarObsevacionesAnalisisVisitas: `${environment.apiUrl}/visitas/analisis/[idvisita]/observaciones`,
  descargarArchivo: `${environment.apiUrl}/visitas/analisis/[idvisita]/archivo`,
};
