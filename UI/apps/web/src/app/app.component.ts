import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@storeflow/design-system';

@Component({
  standalone: true,
  imports: [RouterModule, SharedModule],
  selector: 'app-root',
  template: `<router-outlet></router-outlet>`,
})
export class AppComponent {}
