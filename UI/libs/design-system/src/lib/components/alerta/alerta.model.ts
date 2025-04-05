export interface Alerta {
  descricion: string;
  tipo: TipoAlerta;
}

export enum TipoAlerta {
  Success = 'success',
  Danger = 'error',
}

export enum TipoIcono {
  Success = 'check_circle',
  Danger = 'dangerous',
}

export const Iconos = {
  [TipoAlerta.Success]: TipoIcono.Success,
  [TipoAlerta.Danger]: TipoIcono.Danger,
};
