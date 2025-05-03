import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Informe } from '../informes.model';
import { SharedModule } from '@storeflow/design-system';

@Component({
  selector: 'app-listado-informes',
  standalone: true,
  imports: [SharedModule, CommonModule],
  templateUrl: './listado-informes.component.html',
  styleUrl: './listado-informes.component.scss',
})
export class ListadoInformesComponent {
  informes = input<Informe[]>([]);

  get tieneInformes(): boolean {
    return this.informes().length > 0;
  }
}
