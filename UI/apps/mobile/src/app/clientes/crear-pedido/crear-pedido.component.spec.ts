import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import {
  BrowserAnimationsModule,
  NoopAnimationsModule,
} from '@angular/platform-browser/animations';
import { Router } from '@angular/router';
import { rutasCrearPedido } from '../clientes.constantes';
import { Producto } from '../clientes.model';
import { ClientesUrls } from '../clientes.urls';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';
import { CrearPedidoComponent } from './crear-pedido.component';

describe('CrearPedidoComponent', () => {
  let component: CrearPedidoComponent;
  let fixture: ComponentFixture<CrearPedidoComponent>;
  let httpMock: HttpTestingController;
  let router: Partial<Router>;
  const esperadoProductos: Producto[] = [
    {
      imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
      nombre: 'Leche ',
      precio: 20000,
      codigo: '123456789',
    },
    {
      imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
      nombre: 'Jugo de naranja',
      precio: 10000,
      codigo: '987654321',
    },
  ];
  beforeEach(async () => {
    router = {
      navigateByUrl: jest.fn(),
    };
    TestBed.overrideProvider(Router, {
      useValue: router,
    });
    await TestBed.configureTestingModule({
      imports: [
        CrearPedidoComponent,
        BrowserAnimationsModule,
        NoopAnimationsModule,
      ],
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
    fixture = TestBed.createComponent(CrearPedidoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener el listado de productos al iniciar el componente', () => {
    const peticion = httpMock.expectOne(ClientesUrls.obtenerProductos);
    peticion.flush(esperadoProductos);
    expect(peticion.request.method).toEqual('GET');
    expect(component.store.productos()).toEqual(esperadoProductos);
  });

  it('debe navegar a la ruta correspondiente, cuando se le de click a "tab-crear-pedido"', async () => {
    const tabCrearPedido = fixture.debugElement.queryAll(
      By.css('[data-testid="tab-crear-pedido"]')
    )[1];

    tabCrearPedido.nativeElement.click();

    expect(router.navigateByUrl).toHaveBeenCalledWith(
      rutasCrearPedido.pedidosPendientes
    );
  });
});
