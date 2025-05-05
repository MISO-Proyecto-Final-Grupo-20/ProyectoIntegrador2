import { Component, inject } from '@angular/core';

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
  styleUrl: './clientes.component.scss',
})
export class ClientesComponent {
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

  get descripcion() {
    return $localize`:@@descripcionClientes:Gestiona tus clientes aquí`;
  }

  seleccionarCliente(cliente: Cliente) {
    this.router.navigateByUrl('home/vendedores/clientes/crearPedido');
    this.store.seleccionarCliente(cliente);
  }
}
