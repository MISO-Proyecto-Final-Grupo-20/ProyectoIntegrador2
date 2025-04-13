import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Alerta, AlertaService, TipoAlerta } from '@storeflow/design-system';
import { Fabricante } from '../fabricante.model';
import { MensajesFabricantes } from '../fabricantes.constantes';
import { FabricantesService } from '../fabricantes.service';
import { FabricantesUrls } from '../fabricantes.urls';
import { RegistrarFabricanteComponent } from './registrar-fabricante.component';

describe('RegistrarFabricanteComponent', () => {
  let component: RegistrarFabricanteComponent;
  let fixture: ComponentFixture<RegistrarFabricanteComponent>;
  let alertaService: Partial<AlertaService>;
  let httpMock: HttpTestingController;
  const formulario: Fabricante = {
    correo: 'agusto@yomap.co',
    nombre: 'Augusto',
  };

  beforeEach(async () => {
    alertaService = {
      abrirAlerta: jest.fn(),
    };
    await TestBed.configureTestingModule({
      imports: [RegistrarFabricanteComponent, BrowserAnimationsModule],
      providers: [
        FabricantesService,
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
        {
          provide: AlertaService,
          useValue: alertaService,
        },
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(RegistrarFabricanteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe deshabilitar el boton de "guardar-fabricante" cuando el formulario es invalido', () => {
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-fabricante"]')
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('debe habilitar el boton de "guardar-fabricante" cuando el formulario es valido', () => {
    component.formulario.patchValue(formulario);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-fabricante"]')
    );
    expect(boton.nativeElement.disabled).toBeFalsy();
  });

  it('debe guardarse correctamente el fabricante , cuando se le de click al boton "guardar-fabricante"  ', () => {
    component.formulario.patchValue(formulario);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-guardar-fabricante"]')
    );
    const esperadoAlerta: Alerta = {
      tipo: TipoAlerta.Success,
      descricion: MensajesFabricantes.registroFabricanteExitoso,
    };

    boton.nativeElement.click();
    const peticion = httpMock.expectOne(FabricantesUrls.registrarFabricante);

    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(formulario);
    peticion.flush({});
    expect(alertaService.abrirAlerta).toHaveBeenCalledWith(esperadoAlerta);
    expect(component.formulario.value).toEqual({
      correo: null,
      nombre: null,
    });
  });
});
