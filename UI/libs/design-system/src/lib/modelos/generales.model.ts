export enum TipoCategoria {
  Cliente = 'cliente',
}

export interface Sesion {
  nombre: string;
  apellido: string;
  email: string;
  categoria: TipoCategoria;
}
