import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { By } from '@angular/platform-browser';
import { Alerta, AlertaService, TipoAlerta } from '@storeflow/design-system';
import { MensajesAlertas } from '../../app.constantes';
import { Producto, ProductoSeleccionado } from '../clientes.model';
import { ClientesUrls } from '../clientes.urls';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';
import { ModalAgregarProductoComponent } from './modal-agregar-producto.component';
import { ModalAgregarProductoService } from './modal-agregar-producto.service';

describe('ModalAgregarProductoComponent', () => {
  let component: ModalAgregarProductoComponent;
  let fixture: ComponentFixture<ModalAgregarProductoComponent>;
  let httpMock: HttpTestingController;
  let alerta: Partial<AlertaService>;
  let modalAgregarProductoService: Partial<ModalAgregarProductoService>;
  const producto: Producto = {
    nombre: 'Producto 1',
    precio: 100000,
    codigo: 'P001',
    imagen: 'https://example.com/producto1.jpg',
    seleccionado: false,
  };
  beforeEach(async () => {
    alerta = {
      abrirAlerta: jest.fn(),
    };
    modalAgregarProductoService = {
      cerrarModal: jest.fn(),
    };
    await TestBed.configureTestingModule({
      imports: [ModalAgregarProductoComponent],
      providers: [
        {
          provide: MAT_DIALOG_DATA,
          useValue: producto,
        },
        {
          provide: AlertaService,
          useValue: alerta,
        },
        {
          provide: ModalAgregarProductoService,
          useValue: modalAgregarProductoService,
        },
        ClientesService,
        HttpTestingController,
        ClientesStore,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(ModalAgregarProductoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe sumar 1 a la cantidad de productos, cuando se le de click al "boton-sumar-producto"', () => {
    const botonSumar = fixture.debugElement.query(
      By.css('[data-testid="boton-sumar-producto"]')
    );
    botonSumar.nativeElement.click();
    fixture.detectChanges();
    expect(component.cantidadProductos()).toEqual(11);
  });

  it('Debe restar 1 a la cantidad de productos, cuando se le de click al "boton-restar-producto"', () => {
    const botonSumar = fixture.debugElement.query(
      By.css('[data-testid="boton-restar-producto"]')
    );
    botonSumar.nativeElement.click();
    fixture.detectChanges();
    expect(component.cantidadProductos()).toEqual(9);
  });

  it('Debe llamarse el servicio que valida el inventario, cuando se le de click al "boton-agregar-producto"', () => {
    const esperado: ProductoSeleccionado = {
      ...producto,
      seleccionado: true,
      cantidad: component.cantidadProductos(),
    };
    const botonAgregar = fixture.debugElement.query(
      By.css('[data-testid="boton-agregar-producto"]')
    );
    botonAgregar.nativeElement.click();
    const peticion = httpMock.expectOne(ClientesUrls.validarInventarioProducto);
    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(esperado);
  });

  it('Debe mostrar un mensaje de alerta, cuando el inventario no sea suficiente', () => {
    const esperadoAlerta: Alerta = {
      tipo: TipoAlerta.Danger,
      descricion: `${MensajesAlertas.noHaySuficienteInventario} ${producto.nombre}`,
    };
    const botonAgregar = fixture.debugElement.query(
      By.css('[data-testid="boton-agregar-producto"]')
    );
    botonAgregar.nativeElement.click();
    const peticion = httpMock.expectOne(ClientesUrls.validarInventarioProducto);
    peticion.flush(false);

    expect(alerta.abrirAlerta).toHaveBeenCalledWith(esperadoAlerta);
  });

  it('Debe seleccionar el producto, cuando el inventario sea suficiente y cerrar el modal', () => {
    const esperado: ProductoSeleccionado = {
      ...producto,
      seleccionado: true,
      cantidad: component.cantidadProductos(),
    };
    const botonAgregar = fixture.debugElement.query(
      By.css('[data-testid="boton-agregar-producto"]')
    );
    botonAgregar.nativeElement.click();
    const peticion = httpMock.expectOne(ClientesUrls.validarInventarioProducto);
    peticion.flush(true);
    expect(modalAgregarProductoService.cerrarModal).toHaveBeenCalled();
    expect(component.store.productosSeleccionados()).toEqual([esperado]);
  });
});
