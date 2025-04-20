import { environment } from '../../environments/environment';

export const ProductosUrls = {
  obtenerListadoFabricantes: `${environment.apiUrl}/listaFabricantes`,
  guardarProducto: `${environment.apiUrl}/productos/registrar`,
  cargarProductosMasivo: `${environment.apiUrl}/productos/cargarMasivo`,
  guardarProductosMasivos: `${environment.apiUrl}/productos/guardarMasivo`,
};
