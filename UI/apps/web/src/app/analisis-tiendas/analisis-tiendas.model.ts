import { Archivo } from '@storeflow/design-system';

export interface AnalisisVisita {
  id: number;
  cliente: string;
  fecha: Date;
  hora: string;
  archivo: Archivo;
  observaciones?: string[];
}
