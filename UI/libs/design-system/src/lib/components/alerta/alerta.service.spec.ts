import { TestBed } from '@angular/core/testing';
import { MatSnackBar, MatSnackBarRef } from '@angular/material/snack-bar';
import { Alerta, TipoAlerta } from './alerta.model';

import { AlertaService } from './alerta.service';
describe('AlertaService', () => {
  let service: AlertaService;
  let snackBar: MatSnackBar;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: MatSnackBarRef,
          useValue: {},
        },
        {
          provide: MatSnackBar,
          useValue: {
            openFromComponent: () => {
              return {
                data: 'Hola',
                duration: 3000,
              };
            },
          },
        },
        AlertaService,
      ],
    });
    service = TestBed.inject(AlertaService);
    snackBar = TestBed.inject(MatSnackBar);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('debe llamarse el metodo "openFromComponent" de  "snackBar" cuando se ejecute el metodo "abrirAlerta" ', () => {
    const alerta: Alerta = {
      descricion: 'Hola',
      tipo: TipoAlerta.Success,
    };

    // eslint-disable-next-line @typescript-eslint/no-empty-function
    const spyOpenFromComponent = jest.spyOn(snackBar, 'openFromComponent');

    service.abrirAlerta(alerta);

    expect(spyOpenFromComponent).toHaveBeenCalled();
  });
});
