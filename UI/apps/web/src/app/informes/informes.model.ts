export interface ConsultaInforme {
  vendedor: number;
  fechaInicial: Date;
  fechaFinal: Date;
  producto: number;
}

export interface Informe {
  vendedor: string;
  fechaVenta: Date;
  producto: string;
  cantidad: number;
}
