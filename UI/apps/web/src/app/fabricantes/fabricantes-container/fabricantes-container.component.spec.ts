import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FabricantesContainerComponent } from './fabricantes-container.component';

describe('FabricantesContainerComponent', () => {
  let component: FabricantesContainerComponent;
  let fixture: ComponentFixture<FabricantesContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FabricantesContainerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FabricantesContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
