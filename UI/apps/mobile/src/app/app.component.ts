import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@storeflow/design-system';
import { Utilidades } from './utilidades';

@Component({
  standalone: true,
  imports: [RouterModule, SharedModule],
  selector: 'app-root',
  template: ` <h1>Hola Mundo</h1>
    <h2>h2</h2>
    <h3>h3</h3>
    <button mat-raised-button color="primary">Botón Primario</button>
    <button mat-raised-button color="accent">Botón Acento</button>
    <button mat-raised-button color="warn">Botón Advertencia</button>
    <router-outlet> </router-outlet>`,
})
export class AppComponent {
  constructor() {
    Utilidades.cambiarStatusBar('Light');
  }
}
