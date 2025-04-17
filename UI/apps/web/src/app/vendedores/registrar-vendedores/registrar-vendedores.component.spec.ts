import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Alerta, AlertaService, TipoAlerta } from '@storeflow/design-system';
import { MensajesVendedores } from '../vendedores.constantes';
import { RegistrarVendedor } from '../vendedores.model';
import { VendedoresUrls } from '../vendedores.urls';
import { RegistrarVendedoresComponent } from './registrar-vendedores.component';

describe('RegistrarVendedoresComponent', () => {
  let component: RegistrarVendedoresComponent;
  let fixture: ComponentFixture<RegistrarVendedoresComponent>;
  let alertaService: Partial<AlertaService>;
  let httpMock: HttpTestingController;

  const formulario: RegistrarVendedor = {
    nombre: 'Augusto romero',
    correo: 'agusto@vendedor.co',
    contrasena: '1213',
  };

  beforeEach(async () => {
    alertaService = {
      abrirAlerta: jest.fn(),
    };
    await TestBed.configureTestingModule({
      providers: [
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
        {
          provide: AlertaService,
          useValue: alertaService,
        },
      ],
      imports: [RegistrarVendedoresComponent, BrowserAnimationsModule],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(RegistrarVendedoresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe deshabilitar el boton de "guardar-vendedor" cuando el formulario es invalido', () => {
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-vendedor"]')
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('debe habilitar el boton de "guardar-vendedor" cuando el formulario es valido', () => {
    component.formulario.patchValue(formulario);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-vendedor"]')
    );
    expect(boton.nativeElement.disabled).toBeFalsy();
  });

  it('debe guardarse correctamente el vendedor, cuando se le de click al boton "guardar-vendedor"  ', () => {
    component.formulario.patchValue(formulario);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-vendedor"]')
    );
    const esperadoAlerta: Alerta = {
      tipo: TipoAlerta.Success,
      descricion: MensajesVendedores.registrarVendedorExitoso,
    };

    boton.nativeElement.click();
    const peticion = httpMock.expectOne(VendedoresUrls.registrarVendedor);

    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(formulario);
    peticion.flush({});
    expect(alertaService.abrirAlerta).toHaveBeenCalledWith(esperadoAlerta);
    expect(component.formulario.value).toEqual({
      nombre: null,
      correo: null,
      contrasena: null,
    });
  });
});
