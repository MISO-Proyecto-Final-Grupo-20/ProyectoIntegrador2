import { Component, inject, signal } from '@angular/core';

import { Producto, Vendedor } from '../../app-model';
import { AppService } from '../../app.service';
import { FormularioConsultaInformesComponent } from '../formulario-consulta-informes/formulario-consulta-informes.component';
import { ConsultaInforme, Informe } from '../informes.model';
import { InformesService } from '../informes.service';
import { ListadoInformesComponent } from '../listado-informes/listado-informes.component';

@Component({
  selector: 'app-informes-container',
  standalone: true,
  imports: [FormularioConsultaInformesComponent, ListadoInformesComponent],
  providers: [AppService, InformesService],
  template: `<div class="px-16 py-8 column gap-10 heigth-100">
    <app-formulario-consulta-informes
      [vendedores]="vendedores()"
      [productos]="productos()"
      (consultarInformes)="consultarInformes($event)"
    ></app-formulario-consulta-informes>
    <app-listado-informes
      class="flex-1"
      [informes]="informes()"
    ></app-listado-informes>
  </div> `,
})
export class InformesContainerComponent {
  private appService = inject(AppService);
  private service = inject(InformesService);
  vendedores = signal<Vendedor[]>([]);
  productos = signal<Producto[]>([]);
  informes = signal<Informe[]>([]);

  constructor() {
    this.obtenerVendedores();
    this.obtenerProductos();
  }

  obtenerVendedores() {
    this.appService.obtenerVendedores().subscribe({
      next: (vendedores) => this.vendedores.set(vendedores),
    });
  }

  obtenerProductos() {
    this.appService.obtenerProductos().subscribe({
      next: (productos) => this.productos.set(productos),
    });
  }

  consultarInformes(datosConsulta: ConsultaInforme) {
    this.service.consultarInformes(datosConsulta).subscribe({
      next: (respuesta) => this.informes.set(respuesta),
    });
  }
}
