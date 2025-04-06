import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductosContainerComponent } from './productos-container.component';

describe('ProductosContainerComponent', () => {
  let component: ProductosContainerComponent;
  let fixture: ComponentFixture<ProductosContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductosContainerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductosContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
