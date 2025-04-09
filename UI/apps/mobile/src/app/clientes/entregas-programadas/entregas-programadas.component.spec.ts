import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EntregasProgramadasComponent } from './entregas-programadas.component';

describe('EntregasProgramadasComponent', () => {
  let component: EntregasProgramadasComponent;
  let fixture: ComponentFixture<EntregasProgramadasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EntregasProgramadasComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(EntregasProgramadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
