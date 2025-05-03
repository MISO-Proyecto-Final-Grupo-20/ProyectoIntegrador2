import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogRef } from '@angular/material/dialog';
import { By } from '@angular/platform-browser';
import { AlertaService } from '@storeflow/design-system';
import { Producto } from '../../app-model';
import { EstadoCarga } from '../productos.enum';
import { RegistrarProducto, ResultadoCargaMasiva } from '../productos.model';
import { ProductosService } from '../productos.service';
import { ProductosUrls } from '../productos.urls';
import { RegistrarProductosMasivoComponent } from './registrar-productos-masivo.component';

describe('RegistrarProductosMasivoComponent', () => {
  let component: RegistrarProductosMasivoComponent;
  let fixture: ComponentFixture<RegistrarProductosMasivoComponent>;
  let alerta: Partial<AlertaService>;
  let httpMock: HttpTestingController;

  const productos: Producto[] = [
    {
      id: 1,
      nombre: 'Paca de leche x12 unidades',
      fabricanteAsociado: { id: 2, nombre: 'Alquería S.A.' },
      codigo: 'A7X9B3Q5LZ82MND4VYKCJ6T1W0GFRP',
      imagen:
        'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
      precio: 1000,
    },
  ];

  const resultadoCarga: ResultadoCargaMasiva = {
    errores: ['Alimentos SAS S.A.S.'],
    productos,
  };

  const productosAGuardar: RegistrarProducto[] = [
    {
      ...productos[0],
      fabricanteAsociado: productos[0].fabricanteAsociado.id,
    },
  ];

  beforeEach(async () => {
    alerta = {
      abrirAlerta: jest.fn(),
    };
    TestBed.overrideProvider(AlertaService, {
      useValue: alerta,
    });
    TestBed.overrideProvider(MatDialogRef, {
      useValue: { close: jest.fn() },
    });
    await TestBed.configureTestingModule({
      providers: [
        ProductosService,
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
      imports: [RegistrarProductosMasivoComponent],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(RegistrarProductosMasivoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe mostrarse la seccion de "adjuntar-archivo", cuando el estado de carga sea inicial  ', () => {
    const adjuntarArchivo = fixture.debugElement.query(
      By.css(`[data-testid="adjuntar-archivo"]`)
    );
    expect(adjuntarArchivo).toBeTruthy();
  });

  it('Debe mostrarse la seccion de "cargando-masivo", cuando el estado de carga sea cargando ', () => {
    component.estadoCarga = EstadoCarga.cargando;
    fixture.detectChanges();
    const cargandoMasivo = fixture.debugElement.query(
      By.css('[data-testid="cargando-masivo"]')
    );
    expect(cargandoMasivo).toBeTruthy();
  });

  it('debe deshabilitar el boton de "guardar-producto-masivo" cuando no hayan productos cargados', () => {
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-productos-masivo"]')
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('debe habilitarse el boton de "guardar-producto-masivo" cuando hayan productos cargados', () => {
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-productos-masivo"]')
    );
    component.resultadoCarga.set(resultadoCarga);
    fixture.detectChanges();
    expect(boton.nativeElement.disabled).toBeFalsy();
  });

  it('debe cargar el archivo, cuando se cargue un archivo "producto-cargar-masivo"  ', () => {
    const cargar = fixture.debugElement.query(
      By.css('[data-testid="producto-cargar-masivo"]')
    );
    const file = new File(['archivo'], 'archivo.csv', { type: 'text/csv' });

    const inputElement = cargar.nativeElement;
    Object.defineProperty(inputElement, 'files', {
      value: [file],
    });
    cargar.nativeElement.dispatchEvent(new Event('change'));
    const esperadoBody = new FormData();
    esperadoBody.append('file', file);
    const peticion = httpMock.expectOne(ProductosUrls.cargarProductosMasivo);

    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(esperadoBody);
    peticion.flush(resultadoCarga);
    expect(component.resultadoCarga()).toEqual(resultadoCarga);
  });

  it('debe guardar los productos masivos, cuando se haga click en el boton "guardar-producto-masivo" ', () => {
    component.resultadoCarga.set(resultadoCarga);
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-productos-masivo"]')
    );
    fixture.detectChanges();
    boton.nativeElement.click();

    const peticionGuardar = httpMock.expectOne(
      ProductosUrls.guardarProductosMasivos
    );
    expect(peticionGuardar.request.method).toEqual('POST');
    expect(peticionGuardar.request.body).toEqual(productosAGuardar);
    peticionGuardar.flush({});

    expect(component.dialogRef.close).toHaveBeenCalled();
    expect(alerta.abrirAlerta).toHaveBeenCalledWith(component.alerta);
  });
});
