import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegistrarVisitaComponent } from './registrar-visita.component';

describe('RegistrarVisitaComponent', () => {
  let component: RegistrarVisitaComponent;
  let fixture: ComponentFixture<RegistrarVisitaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrarVisitaComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(RegistrarVisitaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
