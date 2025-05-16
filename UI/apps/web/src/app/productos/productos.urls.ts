import { environment } from '../../environments/environment';

export const ProductosUrls = {
  guardarProducto: `${environment.apiUrl}/compras/productos`,
  cargarProductosMasivo: `${environment.apiUrl}/compras/productos/masivo`,
  guardarProductosMasivos: `${environment.apiUrl}/compras/productos/guardar-masivo`,
};
