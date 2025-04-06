import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AnalisisTiendasContainerComponent } from './analisis-tiendas-container.component';

describe('AnalisisTiendasContainerComponent', () => {
  let component: AnalisisTiendasContainerComponent;
  let fixture: ComponentFixture<AnalisisTiendasContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnalisisTiendasContainerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AnalisisTiendasContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
