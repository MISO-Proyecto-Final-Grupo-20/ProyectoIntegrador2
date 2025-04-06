import { MenuNavegacion } from './home.model';

export const listadoMenuNavegacion: MenuNavegacion[] = [
  {
    titulo: $localize`:@@menuFabricantes:Fabricantes`,
    iconoLista: 'account_circle',
    url: '/fabricantes',
  },
  {
    titulo: $localize`:@@menuProductos:Productos`,
    iconoLista: 'local_grocery_store',
    url: '/productos',
  },
  {
    titulo: $localize`:@@menuVendedores:Vendedores`,
    iconoLista: 'monetization_on',
    url: '/vendedores',
  },
  {
    titulo: $localize`:@@menuPlanesVenta:Planes de venta`,
    iconoLista: 'request_quote',
    url: '/planesVenta',
  },
  {
    titulo: $localize`:@@menuInformes:Informes`,
    iconoLista: 'assignment',
    url: '/informes',
  },
  {
    titulo: $localize`:@@menuComprasBodega:Compras y bodegas`,
    iconoLista: 'shopping_bag',
    url: '/comprasBodega',
  },
  {
    titulo: $localize`:@@menuAnalisisTiendas:Análisis de tiendas`,
    iconoLista: 'slow_motion_video',
    url: '/analisisTiendas',
  },
  {
    titulo: $localize`:@@menuProgramacionRutas:Programación de rutas`,
    iconoLista: 'local_shipping',
    url: '/programacionRutas',
  },
];
