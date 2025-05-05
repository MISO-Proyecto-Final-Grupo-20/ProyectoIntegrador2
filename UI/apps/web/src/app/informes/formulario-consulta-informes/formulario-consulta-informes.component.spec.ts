import { ComponentFixture, TestBed } from '@angular/core/testing';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_NATIVE_DATE_FORMATS,
} from '@angular/material/core';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarAdapter } from '@storeflow/design-system';
import { ConsultaInforme } from '../informes.model';
import { FormularioConsultaInformesComponent } from './formulario-consulta-informes.component';

describe('FormularioConsultaInformesComponent', () => {
  let component: FormularioConsultaInformesComponent;
  let fixture: ComponentFixture<FormularioConsultaInformesComponent>;
  const formulario: ConsultaInforme = {
    vendedor: 1,
    fechaInicial: new Date(1, 1, 2023),
    fechaFinal: new Date(12, 12, 2023),
    producto: 1,
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormularioConsultaInformesComponent, BrowserAnimationsModule],
      providers: [
        { provide: DateAdapter, useClass: CalendarAdapter },
        { provide: MAT_DATE_FORMATS, useValue: MAT_NATIVE_DATE_FORMATS },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(FormularioConsultaInformesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe emitir el evento "consultarInformes" cuando se le de click al "boton-consultar-informes"', () => {
    const spy = jest.spyOn(component.consultarInformes, 'emit');
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-consultar-informes"]')
    );
    component.formulario.setValue(formulario);
    fixture.detectChanges();
    boton.nativeElement.click();
    expect(spy).toHaveBeenCalledWith(formulario);
  });
});
