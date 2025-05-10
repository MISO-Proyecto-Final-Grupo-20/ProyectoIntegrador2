import { Component } from '@angular/core';
import { ClientesComponent } from '../clientes/clientes.component';

@Component({
  selector: 'app-consulta-clientes-shell',
  standalone: true,
  imports: [ClientesComponent],
  template: `<app-clientes
    [descripcion]="descripcion"
    [ruta]="ruta"
  ></app-clientes>`,
})
export class ConsultaClientesShellComponent {
  ruta = 'home/vendedores/clientes/crearPedido';
  descripcion = $localize`:@@descripcionClientes:Gestiona tus clientes aquí`;
}
