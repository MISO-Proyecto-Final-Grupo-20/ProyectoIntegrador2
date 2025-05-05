import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import {
  BrowserAnimationsModule,
  NoopAnimationsModule,
} from '@angular/platform-browser/animations';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';
import { CrearPedidoComponent } from './crear-pedido.component';
import { Producto } from '../../app.model';
import { AppsUrls } from '../../app.urls';

describe('CrearPedidoComponent', () => {
  let component: CrearPedidoComponent;
  let fixture: ComponentFixture<CrearPedidoComponent>;
  let httpMock: HttpTestingController;
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
    const peticion = httpMock.expectOne(AppsUrls.obtenerProductos);
    peticion.flush(esperadoProductos);
    expect(peticion.request.method).toEqual('GET');
    expect(component.store.productos()).toEqual(esperadoProductos);
  });
});
