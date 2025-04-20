import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SharedModule } from '@storeflow/design-system';

@Component({
  selector: 'app-modal-mostrar-errores',
  standalone: true,
  imports: [SharedModule],
  template: `<section>
    <div class="row justify-content-end px-16 pt-12">
      <mat-icon class="color-grey-800 cursor-pointer" mat-dialog-close
        >close</mat-icon
      >
    </div>
    <mat-dialog-content class="p-16">
      <div class="px-16 column gap-4">
        <p class="mat-body-1 color-grey-800">Errores no cargados</p>
        <div class="bg-warn-50 p-16 radius-4">
          <div class="row gap-4">
            <mat-icon class="color-warn">dangerous</mat-icon>
            <ul>
              @for (error of errores; track $index) {
                <li class="color-warn">{{ error }}</li>
              }
            </ul>
          </div>
        </div>
      </div>
    </mat-dialog-content>
  </section>`,
  styleUrl: './modal-mostrar-errores.component.scss',
})
export class ModalMostrarErroresComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public errores: string[]) {}
}
