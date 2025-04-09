import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MenuNavegacionVendedoresComponent } from './menu-navegacion-vendedores.component';

describe('MenuNavegacionVendedoresComponent', () => {
  let component: MenuNavegacionVendedoresComponent;
  let fixture: ComponentFixture<MenuNavegacionVendedoresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MenuNavegacionVendedoresComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MenuNavegacionVendedoresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
