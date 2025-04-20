import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PedidosPendientesComponent } from './pedidos-pendientes.component';

describe('PedidosPendientesComponent', () => {
  let component: PedidosPendientesComponent;
  let fixture: ComponentFixture<PedidosPendientesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PedidosPendientesComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PedidosPendientesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
