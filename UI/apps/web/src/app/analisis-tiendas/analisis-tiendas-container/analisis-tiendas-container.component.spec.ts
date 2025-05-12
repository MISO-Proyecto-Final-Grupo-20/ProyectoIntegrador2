import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AnalisisTiendasContainerComponent } from './analisis-tiendas-container.component';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { AnalisisTiendasUrls } from '../analisis-tiendas.urls';
import {
  AnalisisVisita,
  DatosModalObservaciones,
} from '../analisis-tiendas.model';
import { By } from '@angular/platform-browser';
import { ModalObservacionesService } from '../modal-observaciones/modal-obsevaciones.service';
import { AlertaService, Utilidades } from '@storeflow/design-system';

describe('AnalisisTiendasContainerComponent', () => {
  let component: AnalisisTiendasContainerComponent;
  let fixture: ComponentFixture<AnalisisTiendasContainerComponent>;
  let httpMock: HttpTestingController;
  let modalObservacionesService: Partial<ModalObservacionesService>;
  let alerta: Partial<AlertaService>;

  const analisisVisitas: AnalisisVisita[] = [
    {
      id: 189,
      fecha: new Date(2025, 3, 23),
      hora: '15:37',
      cliente: 'Surtimax',
      archivo: {
        nombre: 'Pedido semana 15',
        tamanio: 123456,
        url: 'https://example.com/video.mp4',
      },
    },
    {
      id: 188,
      fecha: new Date(2025, 3, 23),
      hora: '15:37',
      cliente: 'Alqueria',
      archivo: {
        nombre: 'Pedido semana 15',
        tamanio: 123456,
        url: 'https://example.com/video.mp4',
      },
      observaciones: 'No se pudo realizar la visita',
    },
  ];

  beforeEach(async () => {
    modalObservacionesService = {
      abrirModal: jest.fn(),
    };
    TestBed.overrideProvider(ModalObservacionesService, {
      useValue: modalObservacionesService,
    });
    alerta = {
      abrirAlerta: jest.fn(),
    };
    URL.revokeObjectURL = jest.fn();

    TestBed.overrideProvider(AlertaService, {
      useValue: alerta,
    });
    await TestBed.configureTestingModule({
      providers: [
        HttpTestingController,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
      imports: [AnalisisTiendasContainerComponent],
    }).compileComponents();
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(AnalisisTiendasContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe obtener el listado de analisis de visitas cuando, se cargue el component', () => {
    obtenerAnalisiVisitas();
    expect(component.analisisVisitas).toEqual(analisisVisitas);
  });

  it('Debe mostrarse el "analisis-visitas", cuando tenga analisis de visitas', () => {
    obtenerAnalisiVisitas();
    fixture.detectChanges();
    const analisisVisitasElement = fixture.debugElement.query(
      By.css('[data-testid="analisis-visitas"]')
    );
    expect(analisisVisitasElement).toBeTruthy();
  });

  it('Debe ocultarse el "analisis-visitas", cuando no tenga analisis de visitas', () => {
    const analisisVisitasElement = fixture.debugElement.query(
      By.css('[data-testid="analisis-visitas"]')
    );
    expect(analisisVisitasElement).toBeFalsy();
  });

  it('debe abrir el modal de observaciones cuando se le de click al "boton-abrir-observacion"', () => {
    obtenerAnalisiVisitas();
    const esperado: DatosModalObservaciones = {
      idVisita: analisisVisitas[1].id,
      observaciones: analisisVisitas[1].observaciones,
    };

    fixture.detectChanges();
    const boton = fixture.debugElement.queryAll(
      By.css('[data-testid="boton-abrir-observacion"]')
    )[1];
    boton.nativeElement.click();
    fixture.detectChanges();
    expect(modalObservacionesService.abrirModal).toHaveBeenCalledWith(esperado);
  });

  it('debe descargar el archivo cuando se le de click al "analisis-tiendas-descargar-archivo"', () => {
    const archivo = analisisVisitas[0].archivo;
    const spyDescargar = jest.spyOn(Utilidades, 'descargarArchivoDesdeUrl');
    obtenerAnalisiVisitas();
    fixture.detectChanges();
    const descargarArchivo = fixture.debugElement.query(
      By.css('[data-testid="analisis-tiendas-descargar-archivo"]')
    );
    descargarArchivo.nativeElement.click();
    expect(spyDescargar).toHaveBeenCalledWith(archivo);
  });

  function obtenerAnalisiVisitas() {
    const peticion = httpMock.expectOne(
      AnalisisTiendasUrls.obtenerAnalisisVisitas
    );
    expect(peticion.request.method).toEqual('GET');
    peticion.flush(analisisVisitas);
  }
});
