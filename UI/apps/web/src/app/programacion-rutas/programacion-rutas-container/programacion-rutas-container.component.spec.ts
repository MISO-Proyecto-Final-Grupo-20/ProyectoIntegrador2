import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProgramacionRutasContainerComponent } from './programacion-rutas-container.component';

describe('ProgramacionRutasContainerComponent', () => {
  let component: ProgramacionRutasContainerComponent;
  let fixture: ComponentFixture<ProgramacionRutasContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProgramacionRutasContainerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ProgramacionRutasContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
