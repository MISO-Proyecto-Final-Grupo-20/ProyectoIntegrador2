import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegistrarVisitaComponent } from './registrar-visita.component';
import { AdjuntarEvidenciaService } from '../adjuntar-evidencia/adjuntar-evidencia.service';
import { By } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { VendedoresService } from '../services/vendedores.service';
import { VendedoresStore } from '../state';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_NATIVE_DATE_FORMATS,
} from '@angular/material/core';
import {
  Alerta,
  AlertaService,
  CalendarAdapter,
  TipoAlerta,
} from '@storeflow/design-system';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { VendedoresUrls } from '../vendedores.urls';
import { Cliente, Visita } from '../vendedores.model';
import { MensajesAlertas } from '../../app.constantes';

describe('RegistrarVisitaComponent', () => {
  let component: RegistrarVisitaComponent;
  let fixture: ComponentFixture<RegistrarVisitaComponent>;
  let adjuntarEvidenciaService: Partial<AdjuntarEvidenciaService>;
  let httpMock: HttpTestingController;
  let alerta: Partial<AlertaService>;

  const formulario = {
    fecha: new Date(2025, 10, 23),
    hora: '23:00',
  };
  const cliente: Cliente = {
    id: 1,
    nombre: 'Cliente 1',
    direccion: 'Direccion 1',
  };

  const visitasRegistradas: Visita[] = [
    {
      id: 1,
      fecha: new Date('2023-10-01'),
      hora: '10:00',
      archivo: {
        nombre: 'visita1.pdf',
        tamanio: 231,
      },
    },
    {
      id: 2,
      fecha: new Date('2023-10-02'),
      hora: '15:00',
      archivo: {
        nombre: 'visita2.pdf',
        tamanio: 231,
      },
    },
  ];

  const archivo = new File([], 'archivo.png', { type: 'image/png' });
  beforeEach(async () => {
    alerta = {
      abrirAlerta: jest.fn(),
    };
    adjuntarEvidenciaService = {
      abrirModal: jest.fn(),
    };
    TestBed.overrideProvider(AdjuntarEvidenciaService, {
      useValue: adjuntarEvidenciaService,
    });
    TestBed.overrideProvider(MAT_DATE_FORMATS, {
      useValue: MAT_NATIVE_DATE_FORMATS,
    });
    TestBed.overrideProvider(AlertaService, {
      useValue: alerta,
    });
    await TestBed.configureTestingModule({
      imports: [RegistrarVisitaComponent, BrowserAnimationsModule],
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
    fixture = TestBed.createComponent(RegistrarVisitaComponent);
    component = fixture.componentInstance;
    component.store.seleccionarCliente(cliente);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe abrir el modal de adjuntar evidencia cuando se le de click al "boton-adjuntar-evidencia"', () => {
    const boton = fixture.debugElement.query(
      By.css('[data-testid="boton-adjuntar-evidencia"]')
    );
    boton.nativeElement.click();

    expect(adjuntarEvidenciaService.abrirModal).toHaveBeenCalled();
  });

  it('Debe ocultarse la seccion de "archivo-seleccionado" y mostrar la seccion de "boton-adjuntar-evidencia" , cuando no se haya seleccionado un archivo', () => {
    const archivoSeleccionado = fixture.debugElement.query(
      By.css('[data-testid="archivo-seleccionado"]')
    );
    const botonAdjuntarEvidencia = fixture.debugElement.query(
      By.css('[data-testid="boton-adjuntar-evidencia"]')
    );

    expect(archivoSeleccionado).toBeFalsy();
    expect(botonAdjuntarEvidencia).toBeTruthy();
  });

  it('Debe mostrar la seccion de "archivo-seleccionado" y ocultar la seccion de "boton-adjuntar-evidencia" , cuando se haya seleccionado un archivo', () => {
    component.store.seleccionarArchivo(archivo);

    fixture.detectChanges();
    const archivoSeleccionado = fixture.debugElement.query(
      By.css('[data-testid="archivo-seleccionado"]')
    );
    const botonAdjuntarEvidencia = fixture.debugElement.query(
      By.css('[data-testid="boton-adjuntar-evidencia"]')
    );

    expect(archivoSeleccionado).toBeTruthy();
    expect(botonAdjuntarEvidencia).toBeFalsy();
  });

  it('Debe desactivar el boton de "Registrar Visita" cuando el formulario es invalido', () => {
    const botonRegistrarVisita = fixture.debugElement.query(
      By.css('[data-testid="boton-registrar-visita"]')
    );
    expect(botonRegistrarVisita.nativeElement.disabled).toBeTruthy();
  });

  it('Debe activar el boton de "Registrar Visita" cuando el formulario es valido', () => {
    component.formulario.patchValue(formulario);
    component.store.seleccionarArchivo(archivo);

    fixture.detectChanges();
    const botonRegistrarVisita = fixture.debugElement.query(
      By.css('[data-testid="boton-registrar-visita"]')
    );
    expect(botonRegistrarVisita.nativeElement.disabled).toBeFalsy();
  });

  it('Debe registrar la visita cuando se le de click al boton "Registrar Visita"', () => {
    const esperadoAlerta: Alerta = {
      tipo: TipoAlerta.Success,
      descricion: MensajesAlertas.registroVisitaExitoso,
    };
    component.formulario.patchValue(formulario);
    component.store.seleccionarArchivo(archivo);
    fixture.detectChanges();
    const botonRegistrarVisita = fixture.debugElement.query(
      By.css('[data-testid="boton-registrar-visita"]')
    );
    botonRegistrarVisita.nativeElement.click();
    const url = VendedoresUrls.visitas.replace(
      '[idCliente]',
      cliente.id.toString()
    );
    const peticion = httpMock.expectOne(url);
    expect(peticion.request.method).toEqual('POST');
    const body = peticion.request.body as FormData;
    expect(body.get('fecha')).toBe(formulario.fecha.toString());
    expect(body.get('hora')).toBe(formulario.hora);
    expect(body.get('archivo')).toBe(archivo);
    peticion.flush({});
    expect(alerta.abrirAlerta).toHaveBeenCalledWith(esperadoAlerta);
    expect(component.store.archivoSeleccionado()).toBeNull();
  });

  it('debe obtener el listado de registro de visitas cuando se llame  obtenerRegistroVisitas', () => {
    component.store.obtenerRegistroVisitas();
    const peticion = httpMock.expectOne(
      VendedoresUrls.visitas.replace('[idCliente]', cliente.id.toString())
    );
    peticion.flush(visitasRegistradas);
    expect(peticion.request.method).toEqual('GET');
    expect(component.store.visitasRegistradas()).toEqual(visitasRegistradas);
  });
});
