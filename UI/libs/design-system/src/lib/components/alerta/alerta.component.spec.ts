import { ComponentFixture, TestBed } from '@angular/core/testing';
import {
  MAT_SNACK_BAR_DATA,
  MatSnackBarRef,
} from '@angular/material/snack-bar';
import { AlertaComponent } from './alerta.component';
import { TipoAlerta, TipoIcono } from './alerta.model';

describe('AlertaComponent', () => {
  let component: AlertaComponent;
  let fixture: ComponentFixture<AlertaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AlertaComponent],
      providers: [
        {
          provide: MatSnackBarRef,
          useValue: {},
        },
        {
          provide: MAT_SNACK_BAR_DATA,
          useValue: {},
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AlertaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe el devolver el tipo de icono cuando se llame el metodo "obtenerIcono" ', () => {
    const actual = component.obtenerIcono(TipoAlerta.Success);
    expect(actual).toEqual(TipoIcono.Success);
  });
});
