import { environment } from '../../environments/environment';

export const ClientesUrls = {
  obtenerProductos: `${environment.apiUrl}/compras/productos`,
  validarInventarioProducto: `${environment.apiUrl}/inventarios/existeProducto`,
  crearPedido: `${environment.apiUrl}/ventas/pedidos`,
  obtenerPedidosPendientes: `${environment.apiUrl}/ventas/pedidos/pendientes/`,
  obtenerEntregasProgramadas: `${environment.apiUrl}/logistica/entregasProgramadas/`,
};
