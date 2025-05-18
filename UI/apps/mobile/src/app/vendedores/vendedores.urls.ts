import { environment } from '../../environments/environment';

export const VendedoresUrls = {
  obtenerClientes: `${environment.apiUrl}/usuarios/clientes`,
  crearPedidoCliente: `${environment.apiUrl}/ventas/pedidos/[idCliente]`,
  obtenerPedidosPendientesCliente: `${environment.apiUrl}/ventas/pedidos/pendientes/[idCliente]`,
	visitas: `${environment.apiUrl}/ventas/visitas/[idCliente]`,
  obtenerRutasAsignadas: `${environment.apiUrl}/usuarios/rutasAsignadas`,
};
