import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute, Router } from '@angular/router';
import { Alerta, AlertaService, TipoAlerta } from '@storeflow/design-system';
import { MensajesAlertas } from '../../app.constantes';
import { Cliente } from '../login.model';
import { LoginService } from '../login.service';
import { LoginUrls } from '../login.urls';
import { RegistroClienteComponent } from './registro-cliente.component';

describe('RegistroClienteComponent', () => {
  let component: RegistroClienteComponent;
  let fixture: ComponentFixture<RegistroClienteComponent>;
  let httpMock: HttpTestingController;
  let alertaService: Partial<AlertaService>;
  let router: Partial<Router>;
  const formulario: Cliente = {
    nombre: 'augusto',
    correo: 'augusto@store.co',
    direccion: 'calle 123',
    contrasena: 'Augusto1234!',
  };

  beforeEach(async () => {
    router = {
      navigateByUrl: jest.fn(),
    };
    alertaService = {
      abrirAlerta: jest.fn(),
    };
    await TestBed.configureTestingModule({
      imports: [RegistroClienteComponent, BrowserAnimationsModule],
      providers: [
        LoginService,
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: ActivatedRoute, useValue: {} },
        { provide: Router, useValue: router },
        {
          provide: AlertaService,
          useValue: alertaService,
        },
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(RegistroClienteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe deshabilitar el boton de "guardar-cliente" cuando el formulario es invalido', () => {
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-cliente"]')
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('debe habilitar el boton de "guardar-cliente" cuando el formulario es valido', () => {
    component.formulario.patchValue(formulario);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-cliente"]')
    );
    expect(boton.nativeElement.disabled).toBeFalsy();
  });

  it('debe guardarse correctamente el cliente , cuando se le de click al boton "guardar-cliente"  ', () => {
    component.formulario.patchValue(formulario);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-cliente"]')
    );
    const esperadoAlerta: Alerta = {
      tipo: TipoAlerta.Success,
      descricion: MensajesAlertas.clienteRegistradoExitoso,
    };

    boton.nativeElement.click();
    const peticion = httpMock.expectOne(LoginUrls.registrarCliente);

    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(formulario);
    peticion.flush({});
    expect(alertaService.abrirAlerta).toHaveBeenCalledWith(esperadoAlerta);
    expect(router.navigateByUrl).toHaveBeenCalledWith('/login/cliente');
  });

  it('debe mostrar la seccion de "contrasenaIncorrecta", cuando no tiene el formato de contrasena', () => {
    const control = component.formulario.get('contrasena');
    control?.setValue('12345678');
    control?.markAsTouched();

    fixture.detectChanges();
    const contrasenaIncorrecta = fixture.debugElement.query(
      By.css('[name="contrasenaIncorrecta"]')
    );
    expect(contrasenaIncorrecta).toBeTruthy();
  });

  it('debe ocultarse la seccion de "contrasenaIncorrecta", cuando  tiene el formato de contrasena', () => {
    const control = component.formulario.get('contrasena');
    control?.setValue(formulario.contrasena);
    control?.markAsTouched();

    fixture.detectChanges();
    const contrasenaIncorrecta = fixture.debugElement.query(
      By.css('[data-testid="contrasenaIncorrecta"]')
    );
    expect(contrasenaIncorrecta).toBeFalsy();
  });
});
