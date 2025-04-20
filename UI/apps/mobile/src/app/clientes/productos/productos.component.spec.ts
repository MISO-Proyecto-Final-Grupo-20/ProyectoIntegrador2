import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Producto, ProductoSeleccionado } from '../clientes.model';
import { ClientesUrls } from '../clientes.urls';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ModalCrearPedidoService } from '../modal-crear-pedido/modal-crear-pedido.service';
import { ClientesService } from '../services/clientes.service';
import { ClientesStore } from '../state';
import { ProductosComponent } from './productos.component';

describe('ProductosComponent', () => {
  let component: ProductosComponent;
  let fixture: ComponentFixture<ProductosComponent>;
  let httpMock: HttpTestingController;
  let modalAgregarProducto: Partial<ModalAgregarProductoService>;
  let modalCrearPedido: Partial<ModalCrearPedidoService>;
  const productos: Producto[] = [
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
    {
      imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
      nombre: 'Papas',
      precio: 10000,
      codigo: '23564',
    },
  ];

  beforeEach(async () => {
    modalAgregarProducto = {
      abrirModal: jest.fn(),
    };
    modalCrearPedido = {
      abrirModal: jest.fn(),
    };
    TestBed.overrideProvider(ModalAgregarProductoService, {
      useValue: modalAgregarProducto,
    });
    TestBed.overrideProvider(ModalCrearPedidoService, {
      useValue: modalCrearPedido,
    });
    await TestBed.configureTestingModule({
      imports: [ProductosComponent, BrowserAnimationsModule],
      providers: [
        ClientesStore,
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
        ClientesService,
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(ProductosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    component.store.obtenerProductos();
    const peticion = httpMock.expectOne(ClientesUrls.obtenerProductos);
    peticion.flush(productos);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it.each([
    {
      filtro: 'JU',
      esperado: [productos[1]],
    },
    {
      filtro: '23',
      esperado: [productos[0], productos[2]],
    },
  ])(
    'Debe filtrar los productos por nombre o codigo, cuando cambie el valor de control "input-buscar-producto"',
    fakeAsync((datos: { filtro: string; esperado: Producto[] }) => {
      const buscador = fixture.debugElement.query(
        By.css('[data-testid="input-buscar-producto"]')
      ).nativeElement;
      buscador.value = datos.filtro;
      buscador.dispatchEvent(new Event('input'));
      tick();
      expect(component.store.productosFiltrados()).toEqual(datos.esperado);
    })
  );
  it('debe abrir el modal de agregar producto, cuando se llame el metodo "seleccionarProducto" y no este seleccionado ', () => {
    const producto: Producto = { ...productos[0], seleccionado: false };
    component.seleccionarProducto(producto);
    expect(modalAgregarProducto.abrirModal).toHaveBeenCalledWith(producto);
  });
  it('debe eliminar el producto ya seleccionado, cuando se llame el metodo "seleccionarProducto" y el producto se encuentre seleccionado ', () => {
    const producto: ProductoSeleccionado = {
      ...productos[0],
      seleccionado: true,
      cantidad: 1,
    };
    component.store.seleccionarProducto(producto);
    component.seleccionarProducto(producto);
    expect(component.store.productosSeleccionados()).toEqual([]);
  });

  it('debe desahabilitar el boton de crear pedido, cuando no haya productos seleccionados', () => {
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-ver-carrito"]')
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('debe habilitar el boton de crear pedido, cuando haya productos seleccionados', () => {
    const producto: ProductoSeleccionado = {
      ...productos[0],
      seleccionado: true,
      cantidad: 1,
    };
    component.store.seleccionarProducto(producto);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-ver-carrito"]')
    );
    expect(boton.nativeElement.disabled).toBeFalsy();
  });

  it('debe abrir el modal de crear pedido, cuando se le de click al "boton-ver-carrito" ', () => {
    const producto: ProductoSeleccionado = {
      ...productos[0],
      seleccionado: true,
      cantidad: 1,
    };
    component.store.seleccionarProducto(producto);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-ver-carrito"]')
    );
    boton.nativeElement.click();
    expect(modalCrearPedido.abrirModal).toHaveBeenCalled();
  });
});
