import { Route } from '@angular/router';
import { ProductosContainerComponent } from './productos-container/productos-container.component';

export const ProductosRoutes: Route[] = [
  {
    path: '',
    component: ProductosContainerComponent,
  },
];
