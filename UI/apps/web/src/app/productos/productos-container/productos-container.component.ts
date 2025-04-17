import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RegistrarProductosComponent } from '../registrar-productos/registrar-productos.component';

@Component({
  selector: 'app-productos-container',
  standalone: true,
  imports: [CommonModule, RegistrarProductosComponent],
  templateUrl: './productos-container.component.html',
  styleUrl: './productos-container.component.scss',
})
export class ProductosContainerComponent {}
