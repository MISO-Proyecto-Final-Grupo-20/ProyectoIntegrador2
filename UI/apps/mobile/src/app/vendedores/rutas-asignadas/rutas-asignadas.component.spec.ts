import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RutasAsignadasComponent } from './rutas-asignadas.component';

describe('RutasAsignadasComponent', () => {
  let component: RutasAsignadasComponent;
  let fixture: ComponentFixture<RutasAsignadasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RutasAsignadasComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(RutasAsignadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
