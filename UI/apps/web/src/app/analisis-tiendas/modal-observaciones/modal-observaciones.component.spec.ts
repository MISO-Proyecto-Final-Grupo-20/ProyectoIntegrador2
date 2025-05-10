import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ModalObservacionesComponent } from './modal-observaciones.component';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ModalObservacionesComponent', () => {
  let component: ModalObservacionesComponent;
  let fixture: ComponentFixture<ModalObservacionesComponent>;
  const idVisita = 123;

  beforeEach(async () => {
    TestBed.overrideProvider(MAT_DIALOG_DATA, {
      useValue: idVisita,
    });
    await TestBed.configureTestingModule({
      imports: [ModalObservacionesComponent, BrowserAnimationsModule],
    }).compileComponents();

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
});
