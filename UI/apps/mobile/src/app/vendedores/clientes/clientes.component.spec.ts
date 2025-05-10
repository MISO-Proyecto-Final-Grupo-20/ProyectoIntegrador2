import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { ClientesComponent } from './clientes.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { VendedoresService } from '../services/vendedores.service';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { VendedoresStore } from '../state';
import { provideHttpClient } from '@angular/common/http';
import { Cliente } from '../vendedores.model';
import { By } from '@angular/platform-browser';
import { VendedoresUrls } from '../vendedores.urls';
import { Router } from '@angular/router';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';

describe('ClientesComponent', () => {
  let component: ClientesComponent;
  let fixture: ComponentFixture<ClientesComponent>;
  let httpMock: HttpTestingController;
  let router: Partial<Router>;
  const clientes: Cliente[] = [
    {
      id: 578,
      nombre: 'Surtimax',
      direccion: 'Cr 213 #56- 90 #578',
    },
    {
      id: 576,
      nombre: 'Autoservicio Laguna',
      direccion: 'Cr 213 #56- 90 #578',
    },
    {
      id: 123,
      nombre: 'Autoservicio chapinero',
      direccion: 'Cr 213 #56- 90 #578',
    },
    {
      id: 89,
      nombre: 'Fruver',
      direccion: 'Cr 213 #56- 90 #578',
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
      imports: [ClientesComponent, BrowserAnimationsModule],
      providers: [
        VendedoresService,
        HttpTestingController,
        VendedoresStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        ModalAgregarProductoService,
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(ClientesComponent);
    component = fixture.componentInstance;
    obtenerClientes();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener el listado de clientes al iniciar el componente', () => {
    expect(component.store.clientes()).toEqual(clientes);
  });

  it.each([
    {
      filtro: '12',
      esperado: [clientes[2]],
    },
    {
      filtro: '57',
      esperado: [clientes[0], clientes[1]],
    },
    {
      filtro: 'fru',
      esperado: [clientes[3]],
    },
  ])(
    'Debe filtrar los clientes por nombre y id de cliente, cuando cambie el valor de control "input-buscar-cliente"',
    fakeAsync((datos: { filtro: string; esperado: Cliente[] }) => {
      const buscador = fixture.debugElement.query(
        By.css('[data-testid="input-buscar-cliente"]')
      ).nativeElement;
      buscador.value = datos.filtro;
      buscador.dispatchEvent(new Event('input'));
      tick();
      expect(component.store.clientesFiltrados()).toEqual(datos.esperado);
    })
  );

  it('debe navegar a la ruta de crear pedido, cuando se le de click a "card-cliente"', async () => {
    const esperadoRuta = 'home/vendedores/clientes/crearPedido';
    fixture.componentRef.setInput('ruta', esperadoRuta);
    const cardCliente = fixture.debugElement.queryAll(
      By.css('[data-testid="card-cliente"]')
    )[1];

    cardCliente.nativeElement.click();

    expect(router.navigateByUrl).toHaveBeenCalledWith(esperadoRuta);
  });

  it('debe seleccionar el cliente, cuando se le de click a "card-cliente"', () => {
    const cardCliente = fixture.debugElement.queryAll(
      By.css('[data-testid="card-cliente"]')
    )[1];

    cardCliente.nativeElement.click();

    expect(component.store.clienteSeleccionado()).toEqual(clientes[1]);
  });

  function obtenerClientes() {
    const peticion = httpMock.expectOne(VendedoresUrls.obtenerClientes);
    expect(peticion.request.method).toEqual('GET');
    peticion.flush(clientes);
  }
});
