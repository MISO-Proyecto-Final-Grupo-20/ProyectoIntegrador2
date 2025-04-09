import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { configuracionMenuVendedores } from '../../app.constantes';
import { MenuNavegacionComponent } from '../../shared/menu-navegacion/menu-navegacion.component';

@Component({
  selector: 'app-menu-navegacion-vendedores',
  standalone: true,
  imports: [MenuNavegacionComponent],
  template: `<app-menu-navegacion
    [listadoMenu]="configuracionMenuVendedores"
  ></app-menu-navegacion> `,
})
export class MenuNavegacionVendedoresComponent {
  configuracionMenuVendedores = configuracionMenuVendedores;
}
