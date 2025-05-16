import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegistrarBodegaComponent } from './registrar-bodega.component';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import {
  Alerta,
  AlertaService,
  OpcionesLista,
  TipoAlerta,
} from '@storeflow/design-system';
import { ComprasBodegaService } from '../compras-bodega.service';
import { provideHttpClient } from '@angular/common/http';
import { AppService } from '../../app.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ListadoFabricantes, Producto } from '../../app-model';
import { AppsUrls } from '../../app.urls';
import { ComprasBodegaUrls } from '../compra-bodega.urls';
import { ComprasBodegaStore } from '../state';
import { ModalSeleccionarProductosService } from '../modal-seleccionar-productos/modal-seleccionar-productos.service';
import { By } from '@angular/platform-browser';
import {
  ProductoSeleccionado,
  RegistroCompraBodega,
} from '../compras-bodega.model';
import { MensajesComprasBodegas } from '../compras-bodega.constantes';

describe('RegistrarBodegaComponent', () => {
  let component: RegistrarBodegaComponent;
  let fixture: ComponentFixture<RegistrarBodegaComponent>;
  let alertaService: Partial<AlertaService>;
  let httpMock: HttpTestingController;
  let modalSeleccionarProductosService: Partial<ModalSeleccionarProductosService>;

  const fabricantes: ListadoFabricantes[] = [
    { id: 1, nombre: 'Fabricante 1' },
    { id: 2, nombre: 'Fabricante 2' },
  ];
  const bodegas: OpcionesLista = [
    { id: 1, descripcion: 'Bodega 1' },
    { id: 2, descripcion: 'Bodega 2' },
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

  const formulario = {
    fabricante: 1,
    bodega: 1,
  };

  const productosSeleccionados: ProductoSeleccionado[] = [
    { ...productos[0], cantidad: 1 },
  ];

  beforeEach(async () => {
    alertaService = {
      abrirAlerta: jest.fn(),
    };
    modalSeleccionarProductosService = {
      abrirModal: jest.fn(),
    };
    TestBed.overrideProvider(ModalSeleccionarProductosService, {
      useValue: modalSeleccionarProductosService,
    });
    TestBed.overrideProvider(AlertaService, {
      useValue: alertaService,
    });
    await TestBed.configureTestingModule({
      imports: [RegistrarBodegaComponent, BrowserAnimationsModule],
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
    fixture = TestBed.createComponent(RegistrarBodegaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('debe obtener el listado de fabricantes al inicializar el componente', () => {
    const peticion = httpMock.expectOne(AppsUrls.obtenerListadoFabricantes);
    peticion.flush(fabricantes);
    expect(peticion.request.method).toEqual('GET');
    expect(component.store.listadoFabricantes()).toEqual(fabricantes);
  });

  it('debe obtener el listado de bodegas al inicializar el componente', () => {
    const peticion = httpMock.expectOne(
      ComprasBodegaUrls.obtenerListadoBodegas
    );
    peticion.flush(bodegas);
    expect(peticion.request.method).toEqual('GET');
    expect(component.store.listadoBodegas()).toEqual(bodegas);
  });

  it('debe obtener el listado de productos al inicializar el componente', () => {
    const peticion = httpMock.expectOne(AppsUrls.productos);
    peticion.flush(productos);
    expect(peticion.request.method).toEqual('GET');
    expect(component.store.productos()).toEqual(productos);
  });

  it('debe abrir el modal de seleccionar productos, cuando se le de click al boton "boton-agregar-producto-registrar-bodega"', () => {
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-agregar-producto-registrar-bodega"]')
    );
    boton.nativeElement.click();
    fixture.detectChanges();
    expect(modalSeleccionarProductosService.abrirModal).toHaveBeenCalled();
  });

  it('debe deshabilitar el boton de registrar, cuando el formulario es invalido', () => {
    component.formulario.setValue({
      fabricante: null,
      bodega: null,
    });
    fixture.detectChanges();
    expect(component.desabilitarBotonRegistrar).toBeTruthy();
  });

  it('debe deshabilitar el boton de registrar, cuando no hay productos seleccionados', () => {
    component.formulario.patchValue(formulario);
    fixture.detectChanges();
    expect(component.desabilitarBotonRegistrar).toBeTruthy();
  });

  it('debe habilitar el boton de registrar, cuando el formulario es valido y hay productos seleccionados', () => {
    component.formulario.patchValue(formulario);
    component.store.seleccionarProductos(productosSeleccionados);
    fixture.detectChanges();
    expect(component.desabilitarBotonRegistrar).toBeFalsy();
  });

  it('debe registrar la compra de bodega, cuando se le de click al boton "boton-registrar-bodega"', () => {
    const esperadoAlerta: Alerta = {
      tipo: TipoAlerta.Success,
      descricion: MensajesComprasBodegas.registroCompraExitoso,
    };

    const esperado: RegistroCompraBodega = {
      bodega: 1,
      fabricante: 1,
      productos: [
        {
          id: productosSeleccionados[0].id,
          cantidad: productosSeleccionados[0].cantidad,
        },
      ],
    };
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-registrar-bodega"]')
    );
    component.formulario.patchValue(formulario);
    component.store.seleccionarProductos(productosSeleccionados);
    fixture.detectChanges();
    boton.nativeElement.click();
    const peticion = httpMock.expectOne(ComprasBodegaUrls.bodegas);
    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(esperado);
    peticion.flush({});
    expect(alertaService.abrirAlerta).toHaveBeenCalledWith(esperadoAlerta);
    expect(component.store.productosSeleccionados()).toEqual([]);
    expect(component.formulario.value).toEqual({
      fabricante: null,
      bodega: null,
    });
  });
});
