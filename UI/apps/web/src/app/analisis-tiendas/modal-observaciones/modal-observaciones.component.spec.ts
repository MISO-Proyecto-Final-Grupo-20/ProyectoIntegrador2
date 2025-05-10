import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ModalObservacionesComponent } from './modal-observaciones.component';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { AnalisisTiendasUrls } from '../analisis-tiendas.urls';
import { AlertaService, TipoAlerta } from '@storeflow/design-system';
import { MensajesAnalisisTienda } from '../analisis-tiendas.constantes';

describe('ModalObservacionesComponent', () => {
  let component: ModalObservacionesComponent;
  let fixture: ComponentFixture<ModalObservacionesComponent>;
  let httpMock: HttpTestingController;
  let alerta: Partial<AlertaService>;
  const idVisita = 123;

  beforeEach(async () => {
    TestBed.overrideProvider(MAT_DIALOG_DATA, {
      useValue: idVisita,
    });

    TestBed.overrideProvider(MatDialogRef, {
      useValue: { close: jest.fn() },
    });

    alerta = {
      abrirAlerta: jest.fn(),
    };

    TestBed.overrideProvider(AlertaService, {
      useValue: alerta,
    });
    await TestBed.configureTestingModule({
      imports: [ModalObservacionesComponent, BrowserAnimationsModule],
      providers: [
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(ModalObservacionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe desabilitar el "boton-analisis-tienda-guardar" de guardar, cuando no hay observaciones', () => {
    const botonGuardar = fixture.debugElement.query(
      By.css('[data-testid="boton-analisis-tienda-guardar"]')
    );
    expect(botonGuardar.nativeElement.disabled).toBeTruthy();
  });

  it('Debe habilitar el "boton-analisis-tienda-guardar" de guardar, cuando hay observaciones', () => {
    component.controlObservaciones.setValue('Observaciones');
    fixture.detectChanges();
    const botonGuardar = fixture.debugElement.query(
      By.css('[data-testid="boton-analisis-tienda-guardar"]')
    );
    expect(botonGuardar.nativeElement.disabled).toBeFalsy();
  });

  it('Debe guardar las observaciones cuando se le de click al "boton-analisis-tienda-guardar"', () => {
    const observaciones = 'Observaciones';
    component.controlObservaciones.setValue(observaciones);
    fixture.detectChanges();
    const botonGuardar = fixture.debugElement.query(
      By.css('[data-testid="boton-analisis-tienda-guardar"]')
    );
    botonGuardar.nativeElement.click();
    fixture.detectChanges();
    const url = AnalisisTiendasUrls.guardarObsevacionesAnalisisVisitas.replace(
      '[idVisita]',
      idVisita.toString()
    );
    const peticion = httpMock.expectOne(url);
    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(observaciones);
    peticion.flush({});
    expect(alerta.abrirAlerta).toHaveBeenCalledWith({
      tipo: TipoAlerta.Success,
      descricion: MensajesAnalisisTienda.guardarObservacionesExitoso,
    });
    expect(component.dialogRef.close).toHaveBeenCalled();
  });
});
