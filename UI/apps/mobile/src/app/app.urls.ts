import { environment } from '../environments/environment';

export const AppsUrls = {
  obtenerProductos: `${environment.apiUrl}/compras/productos`,
  validarInventarioProducto: `${environment.apiUrl}/inventarios/existeProducto`,
};
