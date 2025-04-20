import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { ModalMostrarErroresService } from '../modal-mostrar-errores/modal-mostrar-errores.service';
import { ResultadoCargaMasiva } from '../productos.model';
import { ResultadoCargaMasivaComponent } from './resultado-carga-masiva.component';

describe('ResultadoCargaMasivaComponent', () => {
  let component: ResultadoCargaMasivaComponent;
  let fixture: ComponentFixture<ResultadoCargaMasivaComponent>;
  let modalMostrarErroresService: Partial<ModalMostrarErroresService>;
  const productosSimulados: ResultadoCargaMasiva = {
    errores: ['Alimentos SAS S.A.S.'],
    productos: [
      {
        nombre: 'Paca de leche x12 unidades',
        fabricanteAsociado: { id: 2, nombre: 'Alquería S.A.' },
        codigo: 'A7X9B3Q5LZ82MND4VYKCJ6T1W0GFRP',
        imagen:
          'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
        precio: 1000,
      },
    ],
  };

  beforeEach(async () => {
    modalMostrarErroresService = {
      abrirModal: jest.fn(),
    };
    TestBed.overrideProvider(ModalMostrarErroresService, {
      useValue: modalMostrarErroresService,
    });
    await TestBed.configureTestingModule({
      imports: [ResultadoCargaMasivaComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ResultadoCargaMasivaComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('resultadoCarga', productosSimulados);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe abrir el modal de errores cuando se le de click al "boton-ver-errores"', () => {
    const boton = fixture.debugElement.query(
      By.css('[data-testid="boton-ver-errores"]')
    );
    boton.nativeElement.click();

    expect(modalMostrarErroresService.abrirModal).toHaveBeenCalledWith(
      productosSimulados.errores
    );
  });
});
