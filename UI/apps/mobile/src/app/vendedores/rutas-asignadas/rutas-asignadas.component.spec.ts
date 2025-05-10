import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { RutasAsignadasComponent } from './rutas-asignadas.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_NATIVE_DATE_FORMATS,
} from '@angular/material/core';
import { CalendarAdapter } from '@storeflow/design-system';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { VendedoresService } from '../services/vendedores.service';
import { VendedoresStore } from '../state';
import { VendedoresUrls } from '../vendedores.urls';
import { RutaAsignada } from '../vendedores.model';
import { By } from '@angular/platform-browser';

describe('RutasAsignadasComponent', () => {
  let component: RutasAsignadasComponent;
  let fixture: ComponentFixture<RutasAsignadasComponent>;
  let httpMock: HttpTestingController;

  const rutasAsignadas: RutaAsignada[] = [
    {
      cliente: 'Surtimax',
      direccion: 'Cr 238#47- 889',
      fecha: new Date(2025, 5, 3),
    },
    {
      cliente: 'Autoservicio Laguna',
      direccion: 'Cr 238#47- 889',
      fecha: new Date(2025, 5, 4),
    },
    {
      cliente: 'Autoservicio chapinero',
      direccion: 'Cr 238#47- 889',
      fecha: new Date(2025, 5, 5),
    },
    {
      cliente: 'Fruver',
      direccion: 'Cr 238#47- 889',
      fecha: new Date(2025, 5, 4),
    },
  ];

  beforeEach(async () => {
    TestBed.overrideProvider(MAT_DATE_FORMATS, {
      useValue: MAT_NATIVE_DATE_FORMATS,
    });
    await TestBed.configureTestingModule({
      imports: [RutasAsignadasComponent, BrowserAnimationsModule],
      providers: [
        VendedoresService,
        HttpTestingController,
        VendedoresStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        ModalAgregarProductoService,
        { provide: DateAdapter, useClass: CalendarAdapter },
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(RutasAsignadasComponent);
    component = fixture.componentInstance;
    obtenerRutasAsignadas();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe Obtener las rutas asignadas, cuando cargue el componente', () => {
    expect(component.store.rutasAsignadas()).toEqual(rutasAsignadas);
  });

  it.each([
    {
      filtro: new Date(2025, 5, 5),
      esperado: [rutasAsignadas[2]],
    },
    {
      filtro: new Date(2025, 5, 4),
      esperado: [rutasAsignadas[1], rutasAsignadas[3]],
    },
    {
      filtro: new Date(2025, 5, 10),
      esperado: [],
    },
  ])(
    'Debe filtrar las rutas asignadas por fecha, cuando cambie el valor de control "calendar-filtar-fecha-rutas-asignadas" y le de click al boton "boton-filtrar-rutas-asignadas"',
    fakeAsync((datos: { filtro: string; esperado: RutaAsignada[] }) => {
      const buscador = fixture.debugElement.query(
        By.css('[data-testid="calendar-filtar-fecha-rutas-asignadas"]')
      ).nativeElement;
      buscador.value = datos.filtro;
      buscador.dispatchEvent(new Event('input'));
      const boton = fixture.debugElement.query(
        By.css('[data-testid="boton-filtrar-rutas-asignadas"]')
      ).nativeElement;
      boton.click();
      tick();
      expect(component.store.rutasAsignadasPorFecha()).toEqual(datos.esperado);
    })
  );
  function obtenerRutasAsignadas() {
    const peticion = httpMock.expectOne(VendedoresUrls.obtenerRutasAsignadas);
    expect(peticion.request.method).toEqual('GET');
    peticion.flush(rutasAsignadas);
  }
});
