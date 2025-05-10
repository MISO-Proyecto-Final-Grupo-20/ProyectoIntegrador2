import { Component, inject } from '@angular/core';
import { SharedModule } from '@storeflow/design-system';
import { MatDialogRef } from '@angular/material/dialog';
import { VendedoresStore } from '../state';

@Component({
  selector: 'app-adjuntar-evidencia',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './adjuntar-evidencia.component.html',
  styleUrl: './adjuntar-evidencia.component.scss',
})
export class AdjuntarEvidenciaComponent {
  dialogRef = inject(MatDialogRef<AdjuntarEvidenciaComponent>);
  store = inject(VendedoresStore);

  seleccionarArchivo(event: Event) {
    const elemento = event.target as HTMLInputElement;
    const archivo = (elemento?.files || [])[0];
    this.store.seleccionarArchivo(archivo);
    this.dialogRef.close();
  }
}
