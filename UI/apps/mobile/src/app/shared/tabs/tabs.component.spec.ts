import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TabsComponent } from './tabs.component';
import { Router } from '@angular/router';
import { By } from '@angular/platform-browser';
import { Tab } from '../../app.model';

describe('TabsComponent', () => {
  let component: TabsComponent;
  let fixture: ComponentFixture<TabsComponent>;
  let router: Partial<Router>;

  const rutas = {
    productos: 'crearPedido',
    pedidosPendientes: 'pedidosPendientes',
  };

  const configuracionTabs: Tab[] = [
    {
      titulo: 'productos',
      ruta: rutas.productos,
    },
    {
      titulo: 'pedidosPendientes',
      ruta: rutas.pedidosPendientes,
    },
  ];

  beforeEach(async () => {
    router = {
      navigateByUrl: jest.fn(),
    };
    TestBed.overrideProvider(Router, {
      useValue: router,
    });
    await TestBed.configureTestingModule({
      imports: [TabsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(TabsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe navegar a la ruta correspondiente, cuando se le de click a "tab-seleccionar"', async () => {
    fixture.componentRef.setInput('tabs', configuracionTabs);
    fixture.detectChanges();
    const tabSeleccionar = fixture.debugElement.queryAll(
      By.css('[data-testid="tab-seleccionar"]')
    )[1];

    tabSeleccionar.nativeElement.click();

    expect(router.navigateByUrl).toHaveBeenCalledWith(rutas.pedidosPendientes);
  });
});
