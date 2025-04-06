import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';

import { LoginService } from '../login.service';
import { LoginUrls } from '../login.urls';
import { AdminShellComponent } from './admin-shell.component';

describe('AdminShellComponent', () => {
  let component: AdminShellComponent;
  let fixture: ComponentFixture<AdminShellComponent>;
  let httpMock: HttpTestingController;
  const solicitud = { correo: 'hola@gmail.co', contrasena: '123456' };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminShellComponent, BrowserAnimationsModule],
      providers: [
        LoginService,
        provideRouter([]),
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(AdminShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener un token del servicio , cuando se llame el metodo "ingresar"  ', () => {
    component.formulario.patchValue(solicitud);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-ingresar"]')
    );

    boton.nativeElement.click();
    const peticion = httpMock.expectOne(LoginUrls.ingresar);
    expect(peticion.request.method).toEqual('POST');
    expect(peticion.request.body).toEqual(solicitud);
  });

  it('debe deshabilitar el boton de ingresar cuando el formulario es invalido', () => {
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-ingresar"]')
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('debe habilitar el boton de ingresar cuando el formulario es valido', () => {
    component.formulario.patchValue({
      correo: 'agusto@yomap.co',
      contrasena: '123456',
    });
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-ingresar"]')
    );
    expect(boton.nativeElement.disabled).toBeFalsy();
  });
});
