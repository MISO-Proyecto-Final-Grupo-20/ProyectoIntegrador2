import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PanelPedidosComponent } from './panel-pedidos.component';

describe('PanelPedidosComponent', () => {
  let component: PanelPedidosComponent;
  let fixture: ComponentFixture<PanelPedidosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PanelPedidosComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PanelPedidosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
