import { Route } from '@angular/router';
import { AuthGuard } from '@storeflow/design-system';

export const appRoutes: Route[] = [
  {
    path: '',
    loadChildren: () =>
      import('./login/login.routes').then((routes) => routes.LoginRoutes),
  },
  {
    path: 'home',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./home/home.routes').then((routes) => routes.HomeRoutes),
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];
