import { Archivo } from '@storeflow/design-system';

export interface AnalisisVisita {
  id: number;
  cliente: string;
  fecha: Date | string;
  hora: string;
  archivo: Archivo;
  observaciones?: string;
}

export interface DatosModalObservaciones {
  idVisita: number;
  observaciones?: string;
}
