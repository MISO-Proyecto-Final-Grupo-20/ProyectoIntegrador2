import { environment } from '../../environments/environment';

export const ProductosUrls = {
  obtenerListadoFabricantes: `${environment.apiUrl}/compras/fabricantes`,
  guardarProducto: `${environment.apiUrl}/compras/productos`,
  cargarProductosMasivo: `${environment.apiUrl}/productos/cargarMasivo`,
  guardarProductosMasivos: `${environment.apiUrl}/productos/guardarMasivo`,
};
