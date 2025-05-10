import { Component, inject, Inject } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
  AlertaService,
  SharedModule,
  TipoAlerta,
} from '@storeflow/design-system';
import { AnalisisTiendasService } from '../analisis-tiendas.service';
import { MensajesAnalisisTienda } from '../analisis-tiendas.constantes';

@Component({
  selector: 'app-modal-observaciones',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  providers: [AnalisisTiendasService],
  templateUrl: './modal-observaciones.component.html',
  styleUrl: './modal-observaciones.component.scss',
})
export class ModalObservacionesComponent {
  private service = inject(AnalisisTiendasService);
  private alertaService = inject(AlertaService);
  dialogRef = inject(MatDialogRef<ModalObservacionesComponent>);
  controlObservaciones = new FormControl<string | null>(
    null,
    Validators.required
  );
  constructor(@Inject(MAT_DIALOG_DATA) public idVisita: number) {}

  guardarObservaciones() {
    this.service
      .guardarObservaciones(
        this.idVisita,
        this.controlObservaciones.value as string
      )
      .subscribe({
        next: () => {
          this.alertaService.abrirAlerta({
            tipo: TipoAlerta.Success,
            descricion: MensajesAnalisisTienda.guardarObservacionesExitoso,
          });
          this.dialogRef.close();
        },
      });
  }
}
