import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PedidosPendientesContainerComponent } from './pedidos-pendientes-container.component';
import { VendedoresService } from '../services/vendedores.service';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { VendedoresStore } from '../state';
import { provideHttpClient } from '@angular/common/http';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';

describe('PedidosPendientesContainerComponent', () => {
  let component: PedidosPendientesContainerComponent;
  let fixture: ComponentFixture<PedidosPendientesContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PedidosPendientesContainerComponent],
      providers: [
        VendedoresService,
        HttpTestingController,
        VendedoresStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        ModalAgregarProductoService,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PedidosPendientesContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
