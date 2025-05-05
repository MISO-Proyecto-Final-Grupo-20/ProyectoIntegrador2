import { Tab } from '../app.model';

const rutasCrearPedidoVendedor = {
  productos: '/home/vendedores/clientes/crearPedido',
  pedidosPendientes: '/home/vendedores/clientes/crearPedido/pedidosPendientes',
};

export const configuracionTabsCrearPedidoACliente: Tab[] = [
  {
    titulo: $localize`:@@productos:Productos`,
    ruta: rutasCrearPedidoVendedor.productos,
  },
  {
    titulo: $localize`:@@pedidosPendientes:Pedidos pendientes`,
    ruta: rutasCrearPedidoVendedor.pedidosPendientes,
  },
];
