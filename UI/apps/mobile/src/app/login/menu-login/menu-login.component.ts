import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@storeflow/design-system';
import { HeaderComponent } from '../../shared/header/header.component';

@Component({
  selector: 'app-menu-login',
  standalone: true,
  imports: [HeaderComponent, SharedModule, RouterModule],
  templateUrl: './menu-login.component.html',
  styleUrl: './menu-login.component.scss',
})
export class MenuLoginComponent {}
