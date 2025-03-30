import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@storeflow/design-system';

@Component({
  standalone: true,
  imports: [RouterModule, SharedModule],
  selector: 'app-root',
  template: `<h1>Hola Mundo</h1>
    <h2>h2</h2>
    <h3>h3</h3>
    <button mat-raised-button color="primary">Botón Primario</button>
    <button mat-raised-button color="accent">Botón Acento</button>
    <button mat-raised-button color="success">Botón Advertencia</button> `,
})
export class AppComponent {}
