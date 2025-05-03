import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { ConsultarPedidoComponent } from './consultar-pedido.component';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';
import { ClientesUrls } from '../clientes.urls';
import { EstadoPedido } from '../../app.enum';
import { ProductoPedido, Pedido } from '../../app.model';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ConsultarPedidoComponent', () => {
  let component: ConsultarPedidoComponent;
  let fixture: ComponentFixture<ConsultarPedidoComponent>;
  let httpMock: HttpTestingController;

  const productos: ProductoPedido[] = [
    {
      id: 1,
      imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
      nombre: 'Leche ',
      precio: 20000,
      codigo: '123456789',
      cantidad: 10,
    },
  ];
  const pedidos: Pedido[] = [
    {
      numero: 578,
      estado: EstadoPedido.pendiente,
      fechaRegistro: new Date(2025, 3, 23),
      fechaEntrega: new Date(2025, 3, 23),
      lugarEntrega: 'Cr 238#47- 889',
      total: 400000,
      productos,
    },
    {
      numero: 155,
      estado: EstadoPedido.entregado,
      fechaRegistro: new Date(2025, 3, 23),
      fechaEntrega: new Date(2025, 3, 23),
      lugarEntrega: 'Cr 238#47- 889',
      total: 500000,
      productos,
    },
    {
      numero: 579,
      estado: EstadoPedido.entregado,
      fechaRegistro: new Date(2025, 3, 23),
      fechaEntrega: new Date(2025, 3, 23),
      lugarEntrega: 'Cr 238#47- 889',
      total: 500000,
      productos,
    },
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsultarPedidoComponent, BrowserAnimationsModule],
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
    fixture = TestBed.createComponent(ConsultarPedidoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    obtenerPedidos();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener el listado de pedidos al iniciar el componente', () => {
    expect(component.store.pedidos()).toEqual(pedidos);
  });

  it.each([
    {
      filtro: '15',
      esperado: [pedidos[1]],
    },
    {
      filtro: '57',
      esperado: [pedidos[0], pedidos[2]],
    },
  ])(
    'Debe filtrar los pedidos por el numero de pedido, cuando cambie el valor de control "input-buscar-pedido"',
    fakeAsync((datos: { filtro: string; esperado: Pedido[] }) => {
      const buscador = fixture.debugElement.query(
        By.css('[data-testid="input-buscar-pedido"]')
      ).nativeElement;
      buscador.value = datos.filtro;
      buscador.dispatchEvent(new Event('input'));
      tick();
      expect(component.store.pedidosFiltrados()).toEqual(datos.esperado);
    })
  );

  function obtenerPedidos() {
    const peticion = httpMock.expectOne(ClientesUrls.obtenerPedidosPendientes);
    expect(peticion.request.method).toEqual('GET');
    peticion.flush(pedidos);
  }
});
