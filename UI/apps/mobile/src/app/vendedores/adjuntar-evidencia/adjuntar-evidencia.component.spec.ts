import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdjuntarEvidenciaComponent } from './adjuntar-evidencia.component';
import { MatDialogRef } from '@angular/material/dialog';
import { By } from '@angular/platform-browser';
import { VendedoresStore } from '../state';
import { VendedoresService } from '../services/vendedores.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';

describe('AdjuntarEvidenciaComponent', () => {
  let component: AdjuntarEvidenciaComponent;
  let fixture: ComponentFixture<AdjuntarEvidenciaComponent>;

  beforeEach(async () => {
    TestBed.overrideProvider(MatDialogRef, {
      useValue: { close: jest.fn() },
    });

    await TestBed.configureTestingModule({
      imports: [AdjuntarEvidenciaComponent],
      providers: [
        VendedoresStore,
        VendedoresService,
        provideHttpClient(),
        provideHttpClientTesting(),
        ModalAgregarProductoService,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AdjuntarEvidenciaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe seleccionar el archivo y cerrar el modal cuando seleccione un archivo en "input-adjuntar-evidencia"', () => {
    const file = new File([], 'incapacidad.png', {
      type: 'image/png',
    });

    const input = fixture.debugElement.query(
      By.css('[data-testid="input-adjuntar-evidencia"]')
    );
    const inputElement = input.nativeElement;
    Object.defineProperty(inputElement, 'files', {
      value: [file],
    });
    inputElement.dispatchEvent(new Event('change'));

    expect(component.store.archivoSeleccionado()).toEqual(file);
    expect(component.dialogRef.close).toHaveBeenCalled();
  });
});
