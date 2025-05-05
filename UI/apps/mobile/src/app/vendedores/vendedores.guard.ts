import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { VendedoresStore } from './state';

export const ClienteSeleccionadoGuard: CanActivateFn = () => {
  const router = inject(Router);
  const store = inject(VendedoresStore);

  if (store.clienteSeleccionado()) return true;
  router.navigateByUrl('home/vendedores/clientes');
  return false;
};
