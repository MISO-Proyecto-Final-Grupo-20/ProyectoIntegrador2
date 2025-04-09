import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MenuNavegacionClientesComponent } from './menu-navegacion-clientes.component';

describe('MenuNavegacionClientesComponent', () => {
  let component: MenuNavegacionClientesComponent;
  let fixture: ComponentFixture<MenuNavegacionClientesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MenuNavegacionClientesComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MenuNavegacionClientesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
