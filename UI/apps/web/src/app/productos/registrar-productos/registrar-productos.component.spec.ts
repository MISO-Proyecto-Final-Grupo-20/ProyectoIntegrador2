import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Alerta, AlertaService, TipoAlerta } from '@storeflow/design-system';
import { MensajesProductos } from '../productos.contantes';
import { ListadoFabricantes, Producto } from '../productos.model';
import { ProductosService } from '../productos.service';
import { ProductosUrls } from '../productos.urls';
import { RegistrarProductosComponent } from './registrar-productos.component';

describe('RegistrarProductosComponent', () => {
  let component: RegistrarProductosComponent;
  let fixture: ComponentFixture<RegistrarProductosComponent>;
  let alertaService: Partial<AlertaService>;
  let httpMock: HttpTestingController;

  const formulario: Producto = {
    nombre: 'Producto 1',
    fabricanteAsociado: 1,
    codigo: 1213,
    precio: 100,
    imagen: 'https://example.com/image.png',
  };

  beforeEach(async () => {
    alertaService = {
      abrirAlerta: jest.fn(),
    };
    await TestBed.configureTestingModule({
      providers: [
        ProductosService,
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
        {
          provide: AlertaService,
          useValue: alertaService,
        },
      ],
      imports: [RegistrarProductosComponent, BrowserAnimationsModule],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(RegistrarProductosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener el listado de fabricantes al inicializar el componente', () => {
    const fabricantes: ListadoFabricantes[] = [
      { id: 1, nombre: 'Fabricante 1' },
      { id: 2, nombre: 'Fabricante 2' },
    ];

    const peticion = httpMock.expectOne(
      ProductosUrls.obtenerListadoFabricantes
    );
    peticion.flush(fabricantes);
    expect(peticion.request.method).toEqual('GET');
    expect(component.listadoFabricantes()).toEqual(fabricantes);
  });

  it('debe deshabilitar el boton de "guardar-producto" cuando el formulario es invalido', () => {
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-producto"]')
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('debe habilitar el boton de "guardar-producto" cuando el formulario es valido', () => {
    component.formulario.patchValue(formulario);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-producto"]')
    );
    expect(boton.nativeElement.disabled).toBeFalsy();
  });

  it('debe guardarse correctamente el producto, cuando se le de click al boton "guardar-producto"  ', () => {
    component.formulario.patchValue(formulario);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-producto"]')
    );
    const esperadoAlerta: Alerta = {
      tipo: TipoAlerta.Success,
      descricion: MensajesProductos.registroProductoExitoso,
    };

    boton.nativeElement.click();
    const peticion = httpMock.expectOne(ProductosUrls.guardarProducto);

    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(formulario);
    peticion.flush({});
    expect(alertaService.abrirAlerta).toHaveBeenCalledWith(esperadoAlerta);
    expect(component.formulario.value).toEqual({
      nombre: null,
      codigo: null,
      fabricanteAsociado: null,
      precio: null,
      imagen: null,
    });
  });
});
