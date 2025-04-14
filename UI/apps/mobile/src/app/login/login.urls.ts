import { environment } from '../../environments/environment';

export const LoginUrls = {
  ingresar: `${environment.apiUrl}/usuarios/login`,
  registrarCliente: `${environment.apiUrl}/usuarios/cliente`,
};
