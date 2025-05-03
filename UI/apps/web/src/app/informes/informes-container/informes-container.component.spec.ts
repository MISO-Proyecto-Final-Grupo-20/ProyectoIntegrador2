import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_NATIVE_DATE_FORMATS,
} from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarAdapter } from '@storeflow/design-system';
import { Producto, Vendedor } from '../../app-model';
import { AppService } from '../../app.service';
import { AppsUrls } from '../../app.urls';
import { ConsultaInforme, Informe } from '../informes.model';
import { InformesUrls } from '../informes.urls';
import { InformesContainerComponent } from './informes-container.component';

describe('InformesContainerComponent', () => {
  let component: InformesContainerComponent;
  let fixture: ComponentFixture<InformesContainerComponent>;
  let httpMock: HttpTestingController;

  const vendedores: Vendedor[] = [
    { id: 1, nombre: 'Camilo Barretor', correo: 'camilo@barreto.co' },
    { id: 2, nombre: 'Augusto Romero', correo: 'augusto@romero.co' },
    { id: 3, nombre: 'Augusto Marinez', correo: 'augusto@marinez.co' },
  ];

  const productos: Producto[] = [
    {
      id: 1,
      codigo: 'A7X9B3Q5LZ82MND4VYKCJ6T1W0GFRP',
      nombre: 'Paca de leche x12 unidades',
      fabricanteAsociado: { id: 2, nombre: 'Alquería S.A.' },
      imagen:
        'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
      precio: 1000,
    },
  ];

  const informes: Informe[] = [
    {
      vendedor: 'Camilo Barreto',
      producto: 'Paca de leche x12 unidades',
      fechaVenta: new Date(2023, 1, 1),
      cantidad: 10,
    },
  ];

  const formulario: ConsultaInforme = {
    vendedor: 1,
    fechaInicial: new Date(1, 1, 2023),
    fechaFinal: new Date(12, 12, 2023),
    producto: 1,
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InformesContainerComponent, BrowserAnimationsModule],
      providers: [
        AppService,
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: DateAdapter, useClass: CalendarAdapter },
        { provide: MAT_DATE_FORMATS, useValue: MAT_NATIVE_DATE_FORMATS },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(InformesContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener el listado de vendedores al inicializar el componente', () => {
    const peticion = httpMock.expectOne(AppsUrls.vendedores);
    expect(peticion.request.method).toEqual('GET');
    peticion.flush(vendedores);
    expect(component.vendedores()).toEqual(vendedores);
  });

  it('debe obtener el listado de productos al inicializar el componente', () => {
    const peticion = httpMock.expectOne(AppsUrls.productos);
    expect(peticion.request.method).toEqual('GET');
    peticion.flush(productos);
    expect(component.productos()).toEqual(productos);
  });

  it('Debe mandar a consultar los informes segun los datos del formulario, cuando se llame el metodo "consultarInformes"', () => {
    component.consultarInformes(formulario);
    const peticion = httpMock.expectOne(InformesUrls.consultaInformes);
    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(formulario);
    peticion.flush(informes);
    expect(component.informes()).toEqual(informes);
  });
});
