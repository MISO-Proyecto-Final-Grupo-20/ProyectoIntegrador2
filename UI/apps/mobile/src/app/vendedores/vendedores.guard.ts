import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { VendedoresStore } from './state';

export const ClienteSeleccionadoGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
) => {
  const router = inject(Router);
  const store = inject(VendedoresStore);

  if (store.clienteSeleccionado()) return true;

  const redirectUrl = route.data['redirectTo'] || 'home/vendedores/clientes';
  router.navigateByUrl(redirectUrl);
  return false;
};
