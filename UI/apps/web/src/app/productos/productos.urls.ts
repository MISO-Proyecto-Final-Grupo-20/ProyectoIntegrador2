import { environment } from '../../environments/environment';

export const ProductosUrls = {
  obtenerListadoFabricantes: `${environment.apiUrl}/listaFabricantes`,
  guardarProducto: `${environment.apiUrl}/productos/registrar`,
};
