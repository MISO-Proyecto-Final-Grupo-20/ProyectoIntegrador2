import { environment } from '../../environments/environment';

export const ProductosUrls = {
  obtenerListadoFabricantes: `${environment.apiUrl}/compras/fabricantes`,
  guardarProducto: `${environment.apiUrl}/compras/productos`,
  cargarProductosMasivo: `${environment.apiUrl}/compras/productos/masivo`,
  guardarProductosMasivos: `${environment.apiUrl}/compras/productos/guardar-masivo`,
};
