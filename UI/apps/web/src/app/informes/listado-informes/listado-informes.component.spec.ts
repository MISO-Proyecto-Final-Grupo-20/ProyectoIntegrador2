import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ListadoInformesComponent } from './listado-informes.component';
import { Informe } from '../informes.model';
import { By } from '@angular/platform-browser';

describe('ListadoInformesComponent', () => {
  let component: ListadoInformesComponent;
  let fixture: ComponentFixture<ListadoInformesComponent>;

  const informes: Informe[] = [
    {
      vendedor: 'Vendedor 1',
      producto: 'Producto 1',
      fechaVenta: new Date(),
      cantidad: 10,
    },
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListadoInformesComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ListadoInformesComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('informes', [] as Informe[]);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe ocultar la seccion de "listado-informes" y mostrarse la seccion de "informes-empty" , cuando no hallan informes ', () => {
    const listadoInformes = fixture.debugElement.query(
      By.css('[data-testid="listado-informes"]')
    );
    const informesEmpty = fixture.debugElement.query(
      By.css('[data-testid="informes-empty"]')
    );
    expect(listadoInformes).toBeFalsy();
    expect(informesEmpty).toBeTruthy();
  });

  it('debe mostrarse la seccion de "listado-informes" y ocultarse la seccion de "informes-empty", cuando hallan informes', () => {
    fixture.componentRef.setInput('informes', informes);
    fixture.detectChanges();
    const listadoInformes = fixture.debugElement.query(
      By.css('[data-testid="listado-informes"]')
    );
    const informesEmpty = fixture.debugElement.query(
      By.css('[data-testid="informes-empty"]')
    );

    expect(listadoInformes).toBeTruthy();
    expect(informesEmpty).toBeFalsy();
  });
});
