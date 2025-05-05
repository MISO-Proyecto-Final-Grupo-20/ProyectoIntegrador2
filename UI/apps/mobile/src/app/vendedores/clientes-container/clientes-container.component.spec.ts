import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ClientesContainerComponent } from './clientes-container.component';

describe('ClientesContainerComponent', () => {
  let component: ClientesContainerComponent;
  let fixture: ComponentFixture<ClientesContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientesContainerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ClientesContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
