import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AlertaComponent } from './alerta.component';
import { Alerta } from './alerta.model';

@Injectable({ providedIn: 'root' })
export class AlertaService {
  constructor(private snackBar: MatSnackBar) {}

  abrirAlerta(alerta: Alerta, tiempo = 3000) {
    this.snackBar.openFromComponent(AlertaComponent, {
      data: alerta,
      duration: tiempo,
      verticalPosition: 'top',
      horizontalPosition: 'end',
      panelClass: [alerta.tipo],
    });
  }
}
