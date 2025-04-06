import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PlanesVentaContainerComponent } from './planes-venta-container.component';

describe('PlanesVentaContainerComponent', () => {
  let component: PlanesVentaContainerComponent;
  let fixture: ComponentFixture<PlanesVentaContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlanesVentaContainerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PlanesVentaContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
