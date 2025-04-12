import { TipoCategoria } from '@storeflow/design-system';
import { ConfiguracionMenu } from './app.model';

export const DescripcionesCategoria: Record<TipoCategoria, string> = {
  [TipoCategoria.Cliente]: 'Cliente',
  [TipoCategoria.Vendedor]: 'Vendedor',
};

export const configuracionMenuClientes: ConfiguracionMenu[] = [
  {
    titulo: $localize`:@@menuCrearPedido:Crear un pedido`,
    imagen: {
      src: 'assets/images/notas.svg',
      width: 34,
      height: 31,
    },
    ruta: 'clientes/crearPedido',
  },
  {
    titulo: $localize`:@@menuConsultarPedido:Consultar pedido`,
    imagen: { src: 'assets/images/icon-principal.svg', width: 33, height: 29 },
    ruta: 'clientes/consultarPedido',
  },
  {
    titulo: $localize`:@@menuEntregasProgramadas:Entregas programadas`,
    imagen: { src: 'assets/images/calendario.svg', width: 25, height: 28 },
    ruta: 'clientes/entregasProgramadas',
  },
];

export const configuracionMenuVendedores: ConfiguracionMenu[] = [
  {
    titulo: $localize`:@@menuClientes:Clientes`,
    imagen: {
      src: 'assets/images/dinero.svg',
      width: 34,
      height: 31,
    },
    ruta: 'vendedores/clientes',
  },
  {
    titulo: $localize`:@@menuRegistrarVisita:Registrar visita`,
    imagen: { src: 'assets/images/icon-principal.svg', width: 33, height: 29 },
    ruta: 'vendedores/registrarVisita',
  },
  {
    titulo: $localize`:@@menuRutasAsignadas:Rutas asignadas`,
    imagen: { src: 'assets/images/calendario.svg', width: 31, height: 20 },
    ruta: 'vendedores/rutasAsignadas',
  },
];

export const MensajesAlertas = {
  credencialesIncorrectas: $localize`:@@mensajeCredencialesIncorrectas:No se pudo autenticar. Verifica tu correo o contraseña.`,
};
