import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ConsultaClientesShellComponent } from './consulta-clientes-shell.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { VendedoresService } from '../services/vendedores.service';
import { VendedoresStore } from '../state';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ConsultaClientesShellComponent', () => {
  let component: ConsultaClientesShellComponent;
  let fixture: ComponentFixture<ConsultaClientesShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        VendedoresService,
        VendedoresStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        ModalAgregarProductoService,
      ],
      imports: [ConsultaClientesShellComponent, BrowserAnimationsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(ConsultaClientesShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
