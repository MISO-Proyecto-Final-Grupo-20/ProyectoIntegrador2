import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { listadoMenuNavegacion } from '../home.constantes';
import { MenuNavegacionComponent } from './menu-navegacion.component';

describe('MenuNavegacionComponent', () => {
  let component: MenuNavegacionComponent;
  let fixture: ComponentFixture<MenuNavegacionComponent>;
  let router: Partial<Router>;

  beforeEach(async () => {
    router = {
      navigateByUrl: jest.fn(),
    };

    await TestBed.configureTestingModule({
      imports: [MenuNavegacionComponent],
      providers: [{ provide: Router, useValue: router }],
    }).compileComponents();

    fixture = TestBed.createComponent(MenuNavegacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debería renderizar el componente y navegar al hacer clic', async () => {
    const opcion = listadoMenuNavegacion[1];
    const menuNavegacion = fixture.debugElement.queryAll(
      By.css('[data-testid="menu-navegacion"]')
    )[1];

    menuNavegacion.nativeElement.click();

    expect(router.navigateByUrl).toHaveBeenCalledWith(`/home${opcion.url}`);
  });
});
