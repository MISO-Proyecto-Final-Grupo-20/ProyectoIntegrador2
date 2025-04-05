import { Route } from '@angular/router';
import { AdminShellComponent } from './admin-shell/admin-shell.component';

export const HomeRoutes: Route[] = [
  {
    path: '',
    component: AdminShellComponent,
  },
];
