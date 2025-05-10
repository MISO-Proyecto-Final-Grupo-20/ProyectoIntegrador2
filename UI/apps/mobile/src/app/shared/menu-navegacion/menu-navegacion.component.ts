import { Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { SharedModule, Utilidades } from '@storeflow/design-system';
import { ConfiguracionMenu } from '../../app.model';
import { CardInformacionComponent } from '../card-informacion/card-informacion.component';

@Component({
  selector: 'app-menu-navegacion',
  standalone: true,
  imports: [SharedModule, CardInformacionComponent],
  templateUrl: './menu-navegacion.component.html',
})
export class MenuNavegacionComponent {
  listadoMenu = input<ConfiguracionMenu[]>();
  router = inject(Router);

  get sesion() {
    return Utilidades.obtenerSesion();
  }

  get titulo() {
    return $localize`:@@tituloMenuNavegacion:Hola ${this.sesion.nombre}`;
  }

  get descripcion() {
    return $localize`:@@descripcionMenuNavegacion:Gestiona tus pedidos y solicitudes aquí`;
  }

  navegar(ruta: string) {
    this.router.navigateByUrl(`/home/${ruta}`);
  }
}
