import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ModalSeleccionarProductosComponent } from './modal-seleccionar-productos.component';
import { By } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { AppService } from '../../app.service';
import { ComprasBodegaService } from '../compras-bodega.service';
import { ComprasBodegaStore } from '../state';
import { Producto } from '../../app-model';
import { AppsUrls } from '../../app.urls';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatDialogRef } from '@angular/material/dialog';

describe('ModalSeleccionarProductosComponent', () => {
  let component: ModalSeleccionarProductosComponent;
  let fixture: ComponentFixture<ModalSeleccionarProductosComponent>;
  let httpMock: HttpTestingController;
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
    {
      id: 2,
      codigo: 'A7X9B3Q5LZ82MND4VYKCJ6T1W0GFRP',
      nombre: 'Paca de leche x12 unidades',
      fabricanteAsociado: { id: 2, nombre: 'Alquería S.A.' },
      imagen:
        'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
      precio: 2000,
    },
  ];

  beforeEach(async () => {
    TestBed.overrideProvider(MatDialogRef, {
      useValue: { close: jest.fn() },
    });
    await TestBed.configureTestingModule({
      imports: [ModalSeleccionarProductosComponent],
      providers: [
        ComprasBodegaService,
        AppService,
        ComprasBodegaStore,
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(ModalSeleccionarProductosComponent);
    component = fixture.componentInstance;
    component.store.obtenerProductos();
    const peticion = httpMock.expectOne(AppsUrls.productos);
    peticion.flush(productos);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('Debe sumar 1 a la cantidad de productos, cuando se le de click al "boton-sumar-producto"', () => {
    const idProducto = productos[0].id;
    const botonSumar = fixture.debugElement.query(
      By.css('[data-testid="boton-sumar-producto"]')
    );
    botonSumar.nativeElement.click();
    fixture.detectChanges();
    const actual = component.obtenerCantidad(idProducto);
    expect(actual).toEqual(11);
  });

  it('Debe restar 1 a la cantidad de productos, cuando se le de click al "boton-restar-producto"', () => {
    const idProducto = productos[0].id;
    const botonRestar = fixture.debugElement.query(
      By.css('[data-testid="boton-restar-producto"]')
    );
    botonRestar.nativeElement.click();
    fixture.detectChanges();
    const actual = component.obtenerCantidad(idProducto);
    expect(actual).toEqual(9);
  });

  it('Debe agregar el producto a los productos seleccionados,  cuando se marque el checkbox', () => {
    const producto = productos[0];
    const event = { checked: true } as MatCheckboxChange;

    component.seleccionarProducto(producto, event);

    expect(component.productosSeleccionados).toEqual(new Set([producto]));
  });

  it('Debe eliminar el producto de los productos seleccionados, cuando se  desmarque el checkbox', () => {
    const producto = productos[0];

    component.seleccionarProducto(producto, {
      checked: true,
    } as MatCheckboxChange);
    component.seleccionarProducto(producto, {
      checked: false,
    } as MatCheckboxChange);

    expect(component.productosSeleccionados).toEqual(new Set([]));
  });

  it('Debe obtener el total del valor de los productos seleccionado segun su cantidad', () => {
    const botonSumar = fixture.debugElement.query(
      By.css('[data-testid="boton-sumar-producto"]')
    );
    botonSumar.nativeElement.click();

    fixture.detectChanges();
    const event = { checked: true } as MatCheckboxChange;
    component.seleccionarProducto(productos[0], event);
    component.seleccionarProducto(productos[1], event);
    expect(component.totalValorProductosSeleccionados).toBe(31000);
  });

  it('Debe deshabilitar el  "boton-agregar-producto-registrar-bodega", cuando no hay productos seleccionados', () => {
    const boton = fixture.debugElement.query(
      By.css('[data-testid="boton-agregar-producto-registrar-bodega"]')
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('Debe habilitar el  "boton-agregar-producto-registrar-bodega", cuando hay productos seleccionados', () => {
    const event = { checked: true } as MatCheckboxChange;
    component.seleccionarProducto(productos[0], event);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('[data-testid="boton-agregar-producto-registrar-bodega"]')
    );
    expect(boton.nativeElement.disabled).toBeFalsy();
  });

  it('Debe agregar el producto a la lista de productos seleccionados, cuando se le de click al boton "boton-agregar-producto-registrar-bodega"', () => {
    component.cantidadProductosInicial = 2;
    const event = { checked: true } as MatCheckboxChange;
    component.seleccionarProducto(productos[0], event);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('[data-testid="boton-agregar-producto-registrar-bodega"]')
    );
    boton.nativeElement.click();
    fixture.detectChanges();
    expect(component.store.productosSeleccionados()).toEqual([
      {
        ...productos[0],
        cantidad: 2,
      },
    ]);
  });
});
