import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CardInformacionComponent } from './card-informacion.component';

describe('CardInformacionComponent', () => {
  let component: CardInformacionComponent;
  let fixture: ComponentFixture<CardInformacionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardInformacionComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CardInformacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
