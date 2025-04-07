import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComprasBodegaContainerComponent } from './compras-bodega-container.component';

describe('ComprasBodegaContainerComponent', () => {
  let component: ComprasBodegaContainerComponent;
  let fixture: ComponentFixture<ComprasBodegaContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComprasBodegaContainerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ComprasBodegaContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
