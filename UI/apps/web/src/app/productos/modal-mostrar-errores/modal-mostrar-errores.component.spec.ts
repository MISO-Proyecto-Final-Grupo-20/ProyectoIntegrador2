import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalMostrarErroresComponent } from './modal-mostrar-errores.component';

describe('ModalMostrarErroresComponent', () => {
  let component: ModalMostrarErroresComponent;
  let fixture: ComponentFixture<ModalMostrarErroresComponent>;
  const errores = ['Alimentos SAS S.A.S.'];

  beforeEach(async () => {
    TestBed.overrideProvider(MAT_DIALOG_DATA, {
      useValue: errores,
    });
    await TestBed.configureTestingModule({
      imports: [ModalMostrarErroresComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ModalMostrarErroresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
