import { Component } from '@angular/core';
import { ClientesComponent } from '../clientes/clientes.component';

@Component({
  selector: 'app-registrar-visita-container',
  standalone: true,
  imports: [ClientesComponent],
  template: `<app-clientes
    [descripcion]="descripcion"
    [ruta]="ruta"
  ></app-clientes>`,
})
export class RegistrarVisitaContainerComponent {
  ruta = 'home/vendedores/registrarVisitas/cliente';
  descripcion = $localize`:@@descripcionClientesRegistrarVisita:Documenta las visitas a tus clientes aquí`;
}
