import { Route } from '@angular/router';
import { ComprasBodegaContainerComponent } from './compras-bodega-container/compras-bodega-container.component';
import { ComprasBodegaService } from './compras-bodega.service';
import { ComprasBodegaStore } from './state';
import { AppService } from '../app.service';

export const ComprasBodegaRoutes: Route[] = [
  {
    path: '',
    component: ComprasBodegaContainerComponent,
    providers: [ComprasBodegaStore, ComprasBodegaService, AppService],
  },
];
