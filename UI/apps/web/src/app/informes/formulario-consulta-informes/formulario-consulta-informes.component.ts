import { Component, input, output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@storeflow/design-system';
import { Producto, Vendedor } from '../../app-model';
import { ConsultaInforme } from '../informes.model';

@Component({
  selector: 'app-formulario-consulta-informes',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  templateUrl: './formulario-consulta-informes.component.html',
  styleUrl: './formulario-consulta-informes.component.scss',
})
export class FormularioConsultaInformesComponent {
  vendedores = input<Vendedor[]>();
  productos = input<Producto[]>();
  consultarInformes = output<ConsultaInforme>();
  formulario = new FormGroup({
    vendedor: new FormControl<number | null>(null),
    fechaInicial: new FormControl<Date | null>(null),
    fechaFinal: new FormControl<Date | null>(null),
    producto: new FormControl<number | null>(null),
  });

  emitirConsulta() {
    this.consultarInformes.emit(this.formulario.value as ConsultaInforme);
  }
}
