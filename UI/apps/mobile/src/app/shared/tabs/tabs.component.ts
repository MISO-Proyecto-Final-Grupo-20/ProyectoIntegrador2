import { Component, inject, input } from '@angular/core';
import { Tab } from '../../app.model';
import { Router } from '@angular/router';
import { SharedModule } from '@storeflow/design-system';

@Component({
  selector: 'app-tabs',
  standalone: true,
  imports: [SharedModule],
  template: `<nav
      mat-tab-nav-bar
      color="accent"
      class="tab-100"
      [tabPanel]="tabPanel"
    >
      @for (tab of tabs(); track $index) {
        <div
          data-testid="tab-seleccionar"
          class="flex-1"
          name="tab"
          [active]="estaSeleccionado(tab.ruta)"
          (click)="seleccionarTab(tab)"
          mat-tab-link
        >
          {{ tab.titulo }}
        </div>
      }
    </nav>
    <mat-tab-nav-panel #tabPanel></mat-tab-nav-panel> `,
})
export class TabsComponent {
  router = inject(Router);
  tabs = input<Tab[]>([]);

  seleccionarTab(seleccion: Tab) {
    this.router.navigateByUrl(seleccion.ruta);
  }

  estaSeleccionado(url: string) {
    return this.router.url === url;
  }
}
