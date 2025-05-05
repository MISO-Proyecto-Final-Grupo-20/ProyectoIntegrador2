import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogRef } from '@angular/material/dialog';
import { By } from '@angular/platform-browser';
import { Alerta, AlertaService, TipoAlerta } from '@storeflow/design-system';
import { MensajesAlertas } from '../../app.constantes';
import { ClientesUrls } from '../clientes.urls';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';
import { ModalCrearPedidoComponent } from './modal-crear-pedido.component';
import { ProductoSeleccionado, RegistroPedido } from '../../app.model';

describe('ModalCrearPedidoComponent', () => {
  let component: ModalCrearPedidoComponent;
  let fixture: ComponentFixture<ModalCrearPedidoComponent>;
  let httpMock: HttpTestingController;
  let alerta: Partial<AlertaService>;

  const productos: ProductoSeleccionado[] = [
    {
      imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
      nombre: 'Leche ',
      precio: 20000,
      codigo: '123456789',
      cantidad: 2,
    },
    {
      imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
      nombre: 'Jugo de naranja',
      precio: 10000,
      codigo: '987654321',
      cantidad: 1,
    },
    {
      imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
      nombre: 'Papas',
      precio: 10000,
      codigo: '23564',
      cantidad: 1,
    },
  ];

  const esperado: RegistroPedido[] = [
    {
      codigo: productos[0].codigo,
      cantidad: productos[0].cantidad,
      precio: productos[0].precio,
    },
    {
      codigo: productos[1].codigo,
      cantidad: productos[1].cantidad,
      precio: productos[1].precio,
    },
    {
      codigo: productos[2].codigo,
      cantidad: productos[2].cantidad,
      precio: productos[2].precio,
    },
  ];

  beforeEach(async () => {
    alerta = {
      abrirAlerta: jest.fn(),
    };

    TestBed.overrideProvider(AlertaService, {
      useValue: alerta,
    });
    TestBed.overrideProvider(MatDialogRef, {
      useValue: { close: jest.fn() },
    });
    await TestBed.configureTestingModule({
      imports: [ModalCrearPedidoComponent],
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
    fixture = TestBed.createComponent(ModalCrearPedidoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe crear el pedido con los productos seleccionados, cuando se le de click al "boton-crear-pedido" ', () => {
    const esperadoAlerta: Alerta = {
      tipo: TipoAlerta.Success,
      descricion: MensajesAlertas.pedidoCreado,
    };
    component.store.seleccionarProducto(productos[0]);
    component.store.seleccionarProducto(productos[1]);
    component.store.seleccionarProducto(productos[2]);

    const boton = fixture.debugElement.query(
      By.css('[data-testid="boton-crear-pedido"]')
    );
    boton.nativeElement.click();
    const peticion = httpMock.expectOne(ClientesUrls.crearPedido);
    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(esperado);
    peticion.flush({});
    expect(component.dialogRef.close).toHaveBeenCalled();
    expect(component.store.productosSeleccionados()).toEqual([]);
    expect(alerta.abrirAlerta).toHaveBeenCalledWith(esperadoAlerta);
  });
});
