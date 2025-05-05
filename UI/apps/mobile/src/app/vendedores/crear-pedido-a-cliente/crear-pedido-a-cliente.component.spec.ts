import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CrearPedidoAClienteComponent } from './crear-pedido-a-cliente.component';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { VendedoresService } from '../services/vendedores.service';
import { VendedoresStore } from '../state';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';

describe('CrearPedidoAClienteComponent', () => {
  let component: CrearPedidoAClienteComponent;
  let fixture: ComponentFixture<CrearPedidoAClienteComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CrearPedidoAClienteComponent],
      providers: [
        VendedoresService,
        HttpTestingController,
        VendedoresStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        ModalAgregarProductoService,
      ],
    }).compileComponents();
    fixture = TestBed.createComponent(CrearPedidoAClienteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
