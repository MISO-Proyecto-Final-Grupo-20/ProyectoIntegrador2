import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AlertaService, OpcionesLista } from '@storeflow/design-system';
import { Vendedor } from '../../app-model';
import { AppService } from '../../app.service';
import { AppsUrls } from '../../app.urls';
import { RegistroPlanVenta } from '../planes-venta.model';
import { PlanesVentaService } from '../planes-venta.service';
import { PlanesVentaUrls } from '../planes-venta.urls';
import { RegistrarPlanesVentaComponent } from './registrar-planes-venta.component';

describe('RegistrarPlanesVentaComponent', () => {
  let component: RegistrarPlanesVentaComponent;
  let fixture: ComponentFixture<RegistrarPlanesVentaComponent>;
  let alertaService: Partial<AlertaService>;
  let httpMock: HttpTestingController;

  const periodosTiempo: OpcionesLista = [
    {
      id: 1,
      descripcion: 'Mensual',
    },
    {
      id: 2,
      descripcion: 'Anual',
    },
  ];

  const vendedores: Vendedor[] = [
    { id: 1, nombre: 'Camilo Barretor', correo: 'camilo@barreto.co' },
    { id: 2, nombre: 'Augusto Romero', correo: 'augusto@romero.co' },
    { id: 3, nombre: 'Augusto Marinez', correo: 'augusto@marinez.co' },
  ];

  const formulario = {
    periodoTiempo: 1,
    valorVentas: 1000,
  };
  beforeEach(async () => {
    alertaService = {
      abrirAlerta: jest.fn(),
    };
    TestBed.overrideProvider(AlertaService, {
      useValue: alertaService,
    });
    await TestBed.configureTestingModule({
      imports: [RegistrarPlanesVentaComponent, BrowserAnimationsModule],
      providers: [
        AppService,
        PlanesVentaService,
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(RegistrarPlanesVentaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener el listado de periodos de tiempo al inicializar el componente', () => {
    const peticion = httpMock.expectOne(PlanesVentaUrls.obtenerPeriodosTiempo);
    expect(peticion.request.method).toEqual('GET');
    peticion.flush(periodosTiempo);
    expect(component.periodosTiempo()).toEqual(periodosTiempo);
  });

  it('debe obtener el listado de vendedores al inicializar el componente', () => {
    const peticion = httpMock.expectOne(AppsUrls.vendedores);
    expect(peticion.request.method).toEqual('GET');
    peticion.flush(vendedores);
    expect(component.vendedores()).toEqual(vendedores);
  });

  it('debe asignar los controles de vendedores al inicializar el componente', () => {
    const esperado = {
      1: false,
      2: false,
      3: false,
    };
    const peticion = httpMock.expectOne(AppsUrls.vendedores);
    peticion.flush(vendedores);
    const actual = component.controlesVendedores.value;
    expect(actual).toEqual(esperado);
  });

  it('debe desactivar el "boton-registrar-planes-ventas", cuando el formulario es invalido', () => {
    const boton = fixture.debugElement.query(
      By.css("button[data-testid='boton-registrar-plan-ventas']")
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('debe habilitarse el "boton-registrar-planes-ventas", cuando el formulario sea valido y tenga un vendedor seleccionado', () => {
    const peticion = httpMock.expectOne(AppsUrls.vendedores);
    peticion.flush(vendedores);
    llenarFormulario();

    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css("button[data-testid='boton-registrar-plan-ventas']")
    );

    expect(boton.nativeElement.disabled).toBeFalsy();
  });

  it('debe registrarse correctamente el plan de ventas correctamente, cuando se le de click al "boton-registrar-plan-ventas"  ', () => {
    const esperado: RegistroPlanVenta = {
      ...formulario,
      vendedores: [1],
    };
    const peticionVendedores = httpMock.expectOne(AppsUrls.vendedores);
    peticionVendedores.flush(vendedores);
    llenarFormulario();
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-registrar-plan-ventas"]')
    );
    const controlPeriodoTiempo = component.formulario.get('periodoTiempo');

    boton.nativeElement.click();
    const peticion = httpMock.expectOne(PlanesVentaUrls.planesVenta);

    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(esperado);
    peticion.flush({});
    expect(alertaService.abrirAlerta).toHaveBeenCalledWith(
      component.alertaExitosa
    );

    expect(controlPeriodoTiempo?.value).toEqual(null);
  });

  function llenarFormulario() {
    component.formulario.patchValue({
      periodoTiempo: 1,
      valorVentas: 1000,
    });
    const controlesVendedores = component.controlesVendedores.controls;
    const vendedorSeleccionado = Object.keys(controlesVendedores)[0];
    const vendedorSeleccionadoControl =
      controlesVendedores[vendedorSeleccionado];
    vendedorSeleccionadoControl?.setValue(true);
  }
});
