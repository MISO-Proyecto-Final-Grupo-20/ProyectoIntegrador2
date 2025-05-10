import { Component, Inject } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SharedModule } from '@storeflow/design-system';

@Component({
  selector: 'app-modal-observaciones',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  templateUrl: './modal-observaciones.component.html',
  styleUrl: './modal-observaciones.component.scss',
})
export class ModalObservacionesComponent {
  controlObservaciones = new FormControl<string | null>(
    null,
    Validators.required
  );
  constructor(@Inject(MAT_DIALOG_DATA) public idVisita: number) {}
}
