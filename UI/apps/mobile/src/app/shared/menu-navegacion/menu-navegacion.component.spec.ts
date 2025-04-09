import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { configuracionMenuClientes } from '../../app.constantes';
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

  it('debe navegar a la ruta segun su seleccion, cuando se le de click al "menu-navegacion-mobile" ', () => {
    fixture.componentRef.setInput('listadoMenu', configuracionMenuClientes);
    fixture.detectChanges();
    const ruta = `/home/${configuracionMenuClientes[1].ruta}`;
    const menuNavegacion = fixture.debugElement.queryAll(
      By.css('[data-testid="menu-navegacion-mobile"]')
    )[1];

    menuNavegacion.nativeElement.click();

    expect(router.navigateByUrl).toHaveBeenCalledWith(ruta);
  });
});
