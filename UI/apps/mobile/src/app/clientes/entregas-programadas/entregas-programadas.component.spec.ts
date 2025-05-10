import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { EntregasProgramadasComponent } from './entregas-programadas.component';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { EntregaProgramada } from '../clientes.model';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';
import { provideHttpClient } from '@angular/common/http';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ClientesUrls } from '../clientes.urls';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('EntregasProgramadasComponent', () => {
  let component: EntregasProgramadasComponent;
  let fixture: ComponentFixture<EntregasProgramadasComponent>;
  let httpMock: HttpTestingController;

  const entregasProgramadas: EntregaProgramada[] = [
    {
      numero: 7669,
      fechaEntrega: new Date(2025, 3, 22),
      lugarEntrega: 'Venecia',
    },
    {
      numero: 1234,
      fechaEntrega: new Date(2025, 3, 22),
      lugarEntrega: 'Cali',
    },
    {
      numero: 7667,
      lugarEntrega: 'Cr 238#47- 889',
    },
    {
      numero: 423,
      lugarEntrega: 'Bogota',
    },
  ] as EntregaProgramada[];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EntregasProgramadasComponent, BrowserAnimationsModule],
      providers: [
        ClientesService,
        HttpTestingController,
        ClientesStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        ModalAgregarProductoService,
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(EntregasProgramadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    obtenerEntregas();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener el listado de entregas programadas al iniciar el componente', () => {
    expect(component.store.entregasProgramadas()).toEqual(entregasProgramadas);
  });

  it.each([
    {
      filtro: '76',
      esperado: [entregasProgramadas[0], entregasProgramadas[2]],
    },
    {
      filtro: '12',
      esperado: [entregasProgramadas[1]],
    },
    {
      filtro: '8912',
      esperado: [],
    },
  ])(
    'Debe filtrar las entregas por el numero de entrega, cuando cambie el valor de control "input-buscar-entrega"',
    fakeAsync((datos: { filtro: string; esperado: EntregaProgramada[] }) => {
      const buscador = fixture.debugElement.query(
        By.css('[data-testid="input-buscar-entrega"]')
      ).nativeElement;
      buscador.value = datos.filtro;
      buscador.dispatchEvent(new Event('input'));
      tick();
      expect(component.store.entregasFiltradas()).toEqual(datos.esperado);
    })
  );

  function obtenerEntregas() {
    const peticion = httpMock.expectOne(
      ClientesUrls.obtenerEntregasProgramadas
    );
    peticion.flush(entregasProgramadas);
    expect(peticion.request.method).toEqual('GET');
  }
});
