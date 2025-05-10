import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegistrarVisitaContainerComponent } from './registrar-visita-container.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { VendedoresService } from '../services/vendedores.service';
import { VendedoresStore } from '../state';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('RegistrarVisitaContainerComponent', () => {
  let component: RegistrarVisitaContainerComponent;
  let fixture: ComponentFixture<RegistrarVisitaContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        VendedoresService,
        VendedoresStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        ModalAgregarProductoService,
      ],
      imports: [RegistrarVisitaContainerComponent, BrowserAnimationsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(RegistrarVisitaContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
