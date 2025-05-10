import { Component, inject, input } from '@angular/core';

import { CardInformacionComponent } from '../../shared/card-informacion/card-informacion.component';
import { SharedModule } from '@storeflow/design-system';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { VendedoresStore } from '../state';
import { Cliente } from '../vendedores.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-clientes',
  standalone: true,
  imports: [CardInformacionComponent, SharedModule, ReactiveFormsModule],
  templateUrl: './clientes.component.html',
})
export class ClientesComponent {
  descripcion = input<string>();
  ruta = input<string>();
  store = inject(VendedoresStore);
  router = inject(Router);
  controlBuscar = new FormControl('');

  constructor() {
    this.store.obtenerClientes();
    this.controlBuscar.valueChanges.subscribe((valor) =>
      this.store.asignarFiltroClientes(valor ?? '')
    );
  }

  get titulo() {
    return $localize`:@@clientes:Clientes`;
  }

  seleccionarCliente(cliente: Cliente) {
    this.router.navigateByUrl(this.ruta() as string);
    this.store.seleccionarCliente(cliente);
  }
}
