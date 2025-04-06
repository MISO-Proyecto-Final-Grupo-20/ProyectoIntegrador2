import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import {
  MAT_SNACK_BAR_DATA,
  MatSnackBarRef,
} from '@angular/material/snack-bar';
import { SharedModule } from '../../modulos/shared-module';
import { Alerta, Iconos, TipoAlerta } from './alerta.model';

@Component({
  selector: 'app-alerta',
  standalone: true,
  imports: [CommonModule, SharedModule],
  templateUrl: './alerta.component.html',
})
export class AlertaComponent {
  constructor(
    public snackBarRef: MatSnackBarRef<AlertaComponent>,
    @Inject(MAT_SNACK_BAR_DATA) public data: Alerta
  ) {}

  get icono(): string {
    return this.obtenerIcono(this.data.tipo);
  }

  obtenerIcono(tipoAlerta: string) {
    return Iconos[tipoAlerta as TipoAlerta];
  }
}
