import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AlertaService,
  TipoAlerta,
  TipoCategoria,
} from '@storeflow/design-system';
import { MensajesAlertas } from '../../app.constantes';
import { LoginService } from '../login.service';
import { LoginUrls } from '../login.urls';
import { LoginClienteComponent } from './login-cliente.component';

describe('LoginClienteComponent', () => {
  let component: LoginClienteComponent;
  let fixture: ComponentFixture<LoginClienteComponent>;
  let httpMock: HttpTestingController;
  let router: Partial<Router>;
  let alertaService: Partial<AlertaService>;
  const solicitud = { correo: 'hola@gmail.co', contrasena: '123456' };
  beforeEach(async () => {
    alertaService = {
      abrirAlerta: jest.fn(),
    };
    router = {
      navigateByUrl: jest.fn(),
    };
    await TestBed.configureTestingModule({
      imports: [LoginClienteComponent, BrowserAnimationsModule],
      providers: [
        LoginService,
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: Router, useValue: router },
        { provide: ActivatedRoute, useValue: {} },
        {
          provide: AlertaService,
          useValue: alertaService,
        },
      ],
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(LoginClienteComponent);
    component = fixture.componentInstance;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener un token del servicio , cuando se llame el metodo "ingresar"  ', () => {
    component.ingresar(solicitud);
    const peticion = httpMock.expectOne(LoginUrls.ingresar);
    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual({
      datosIngreso: solicitud,
      tipoCategoria: TipoCategoria.Cliente,
    });
  });

  it('debe redirigir a la ruta "/home/clientes" cuando se llame el metodo "ingresar" y el servicio devuelva un token', () => {
    component.ingresar(solicitud);
    const peticion = httpMock.expectOne(LoginUrls.ingresar);
    peticion.flush({
      token:
        'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTYifQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c',
    });

    expect(router.navigateByUrl).toHaveBeenCalledWith('/home/clientes');
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
