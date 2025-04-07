import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InformesContainerComponent } from './informes-container.component';

describe('InformesContainerComponent', () => {
  let component: InformesContainerComponent;
  let fixture: ComponentFixture<InformesContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InformesContainerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(InformesContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
