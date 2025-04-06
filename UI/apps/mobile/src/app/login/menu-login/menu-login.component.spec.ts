import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router, RouterLink } from '@angular/router';

import { By } from '@angular/platform-browser';
import { LoginRoutes } from '../login.routes';
import { MenuLoginComponent } from './menu-login.component';
describe('MenuLoginComponent', () => {
  let component: MenuLoginComponent;
  let fixture: ComponentFixture<MenuLoginComponent>;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MenuLoginComponent, RouterLink],
      providers: [provideRouter(LoginRoutes)],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
    router = TestBed.inject(Router);
    fixture = TestBed.createComponent(MenuLoginComponent);
    component = fixture.componentInstance;
    jest.spyOn(router, 'navigateByUrl');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('debe navegar a la ruta "cliente" cuando se haga clic en el botón "boton-cliente"', () => {
    const clienteButton = fixture.debugElement.query(
      By.css('[data-testid="boton-cliente"]')
    );
    clienteButton.nativeElement.click();

    expect(TestBed.inject(Router).url).toBe('/cliente');
  });
});
