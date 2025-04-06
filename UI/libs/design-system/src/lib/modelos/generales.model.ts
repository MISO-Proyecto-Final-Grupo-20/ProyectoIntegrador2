export enum TipoCategoria {
  Cliente = 'cliente',
  Vendedor = 'vendedor',
}

export interface Sesion {
  nombreUsuario: string;
  email: string;
  categoria: TipoCategoria;
}
