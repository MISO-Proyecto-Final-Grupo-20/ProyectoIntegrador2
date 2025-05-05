import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AgregarProductoComponent } from './agregar-producto.component';
import { By } from '@angular/platform-browser';

describe('AgregarProductoComponent', () => {
  let component: AgregarProductoComponent;
  let fixture: ComponentFixture<AgregarProductoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AgregarProductoComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AgregarProductoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Debe sumar 1 a la cantidad de productos, cuando se le de click al "boton-sumar-producto"', () => {
    const botonSumar = fixture.debugElement.query(
      By.css('[data-testid="boton-sumar-producto"]')
    );
    botonSumar.nativeElement.click();
    fixture.detectChanges();
    expect(component.cantidadProductos()).toEqual(11);
  });

  it('Debe restar 1 a la cantidad de productos, cuando se le de click al "boton-restar-producto"', () => {
    const botonSumar = fixture.debugElement.query(
      By.css('[data-testid="boton-restar-producto"]')
    );
    botonSumar.nativeElement.click();
    fixture.detectChanges();
    expect(component.cantidadProductos()).toEqual(9);
  });

  it('Debe emitir la cantidad de productos, cuando se le de click al "boton-agregar-producto"', () => {
    const spy = jest.spyOn(component.agregarProducto, 'emit');

    const botonSumar = fixture.debugElement.query(
      By.css('[data-testid="boton-restar-producto"]')
    );
    botonSumar.nativeElement.click();

    const botonAgregar = fixture.debugElement.query(
      By.css('[data-testid="boton-agregar-producto"]')
    );

    botonAgregar.nativeElement.click();
    fixture.detectChanges();
    expect(spy).toHaveBeenCalledWith(9);
  });
});
