import { TabsCrearPedido } from './clientes.model';

export const rutasCrearPedido = {
  productos: '/home/clientes/crearPedido',
  pedidosPendientes: '/home/clientes/crearPedido/pedidosPendientes',
};

export const configuracionTabsCrearPedido: TabsCrearPedido[] = [
  {
    titulo: $localize`:@@productos:Productos`,
    ruta: rutasCrearPedido.productos,
  },
  {
    titulo: $localize`:@@pedidosPendientes:Pedidos pendientes`,
    ruta: rutasCrearPedido.pedidosPendientes,
  },
];
