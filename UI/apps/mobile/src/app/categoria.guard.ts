import { inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { TipoCategoria, Utilidades } from '@storeflow/design-system';
@Injectable({ providedIn: 'root' })
export class CategoriaGuard implements CanActivate {
  router = inject(Router);

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const sesion = Utilidades.obtenerSesion();
    const categoria = sesion?.categoria;
    const ruta = route.routeConfig?.path;

    if (!categoria || !ruta) {
      this.router.navigateByUrl('/login');
      return false;
    }

    const esCliente = categoria === TipoCategoria.Cliente;
    const esVendedor = categoria === TipoCategoria.Vendedor;

    const accesoValido =
      (ruta === 'clientes' && esCliente) ||
      (ruta === 'vendedores' && esVendedor);

    if (!accesoValido) {
      const rutaRedireccion = esCliente ? '/home/clientes' : '/home/vendedores';
      this.router.navigateByUrl(rutaRedireccion);
      return false;
    }

    return true;
  }
}
