import { Component, input, output } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
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
    vendedor: new FormControl<number | null>(null, Validators.required),
    fechaInicial: new FormControl<Date | null>(null, Validators.required),
    fechaFinal: new FormControl<Date | null>(null, Validators.required),
    producto: new FormControl<number | null>(null, Validators.required),
  });

  emitirConsulta() {
    this.consultarInformes.emit(this.formulario.value as ConsultaInforme);
    this.formulario.reset();
  }
}
