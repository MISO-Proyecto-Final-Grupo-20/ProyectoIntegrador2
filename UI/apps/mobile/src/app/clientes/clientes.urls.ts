import { environment } from '../../environments/environment';

export const ClientesUrls = {
  obtenerProductos: `${environment.apiUrl}/productos`,
  validarInventarioProducto: `${environment.apiUrl}/productos/existeProducto`,
  crearPedido: `${environment.apiUrl}/pedidos`,
};
