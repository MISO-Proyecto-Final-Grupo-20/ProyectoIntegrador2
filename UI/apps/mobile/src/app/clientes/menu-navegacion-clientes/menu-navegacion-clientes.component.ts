import { Component } from '@angular/core';
import { configuracionMenuClientes } from '../../app.constantes';
import { MenuNavegacionComponent } from '../../shared/menu-navegacion/menu-navegacion.component';

@Component({
  selector: 'app-menu-navegacion-clientes',
  standalone: true,
  imports: [MenuNavegacionComponent],
  template: `<app-menu-navegacion
    [listadoMenu]="configuracionMenuClientes"
  ></app-menu-navegacion> `,
})
export class MenuNavegacionClientesComponent {
  configuracionMenuClientes = configuracionMenuClientes;
}
