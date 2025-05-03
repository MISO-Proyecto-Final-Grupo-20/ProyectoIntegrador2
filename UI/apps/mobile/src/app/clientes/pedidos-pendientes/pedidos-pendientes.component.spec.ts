import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PedidosPendientesComponent } from './pedidos-pendientes.component';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ClientesUrls } from '../clientes.urls';
import { Pedido, ProductoPedido } from '../../app.model';
import { EstadoPedido } from '../../app.enum';

describe('PedidosPendientesComponent', () => {
  let component: PedidosPendientesComponent;
  let fixture: ComponentFixture<PedidosPendientesComponent>;
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
  const esperadoPedidos: Pedido[] = [
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
      numero: 555,
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
      imports: [PedidosPendientesComponent],
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
    fixture = TestBed.createComponent(PedidosPendientesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener el listado de pedidos al iniciar el componente', () => {
    const peticion = httpMock.expectOne(ClientesUrls.obtenerPedidosPendientes);
    expect(peticion.request.method).toEqual('GET');
    peticion.flush(esperadoPedidos);
    expect(component.store.pedidos()).toEqual(esperadoPedidos);
  });

  it('Debe obtenerse el listado de pedidos pendientes', () => {
    const esperado = [esperadoPedidos[0]];
    const peticion = httpMock.expectOne(ClientesUrls.obtenerPedidosPendientes);
    peticion.flush(esperadoPedidos);
    expect(component.store.pedidosPendientes()).toEqual(esperado);
  });
});
