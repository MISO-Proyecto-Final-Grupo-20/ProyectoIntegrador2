import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter, Router } from '@angular/router';
import {
  AlertaService,
  TipoAlerta,
  TipoCategoria,
} from '@storeflow/design-system';
import { MensajesAlertas } from '../../app.constantes';
import { LoginService } from '../login.service';
import { LoginUrls } from '../login.urls';
import { LoginVendedorComponent } from './login-vendedor.component';

describe('LoginVendedorComponent', () => {
  let component: LoginVendedorComponent;
  let fixture: ComponentFixture<LoginVendedorComponent>;
  let httpMock: HttpTestingController;
  let router: Partial<Router>;
  let alertaService: Partial<AlertaService>;
  const solicitud = { correo: 'hola@gmail.co', contrasena: '123456' };

  beforeEach(async () => {
    alertaService = {
      abrirAlerta: jest.fn(),
    };
    await TestBed.configureTestingModule({
      imports: [LoginVendedorComponent, BrowserAnimationsModule],
      providers: [
        LoginService,
        provideRouter([]),
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
        {
          provide: AlertaService,
          useValue: alertaService,
        },
      ],
    }).compileComponents();
    router = TestBed.inject(Router);
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(LoginVendedorComponent);
    component = fixture.componentInstance;
    jest.spyOn(router, 'navigateByUrl');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener un token del servicio , cuando se llame el metodo "ingresar" y que tipo de categoria sea vendedor ', () => {
    component.ingresar(solicitud);
    const peticion = httpMock.expectOne(LoginUrls.ingresar);
    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual({
      datosIngreso: solicitud,
      tipoCategoria: TipoCategoria.Vendedor,
    });
  });

  it('debe redirigir a la ruta "/home/vendedores" cuando se llame el metodo "ingresar" y el servicio devuelva un token', () => {
    component.ingresar(solicitud);
    const peticion = httpMock.expectOne(LoginUrls.ingresar);
    peticion.flush({
      token:
        'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTYifQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c',
    });

    expect(router.navigateByUrl).toHaveBeenCalledWith('/home/vendedores');
  });

  it('debe abrir la alerta con mensaje de error de credenciale, cuando el metodo "ingresar" sea status 401', () => {
    component.ingresar(solicitud);
    const peticion = httpMock.expectOne(LoginUrls.ingresar);
    peticion.flush(
      { message: 'Unauthorized' },
      { status: 401, statusText: 'Unauthorized' }
    );

    expect(alertaService.abrirAlerta).toHaveBeenCalledWith({
      tipo: TipoAlerta.Danger,
      descricion: MensajesAlertas.credencialesIncorrectas,
    });
  });
});
