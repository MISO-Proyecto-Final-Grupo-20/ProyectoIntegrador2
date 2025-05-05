import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductosComponent } from './productos.component';
import { By } from '@angular/platform-browser';
import { Producto } from '../../app.model';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ProductosComponent', () => {
  let component: ProductosComponent;
  let fixture: ComponentFixture<ProductosComponent>;
  const productos: Producto[] = [
    {
      imagen: 'https://i.ibb.co/Qvcf4M7R/Leche.png',
      nombre: 'Leche ',
      precio: 20000,
      codigo: '123456789',
    },
    {
      imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
      nombre: 'Jugo de naranja',
      precio: 10000,
      codigo: '987654321',
    },
    {
      imagen: 'https://i.ibb.co/BVxrgLNY/jugo.png',
      nombre: 'Papas',
      precio: 10000,
      codigo: '23564',
    },
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductosComponent, BrowserAnimationsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductosComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('productos', productos);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('debe emitir el evento "filtarProducto" cuando se escriba en el "input-buscar-producto"', () => {
    const filtro = 'filtro';
    const spy = jest.spyOn(component.filtarProducto, 'emit');
    const buscador = fixture.debugElement.query(
      By.css('[data-testid="input-buscar-producto"]')
    ).nativeElement;
    buscador.value = filtro;
    buscador.dispatchEvent(new Event('input'));
    expect(spy).toHaveBeenCalledWith(filtro);
  });

  it('debe emitir el evento "seleccionarProducto" cuando se le de click al "lista-seleccionar-producto"', () => {
    const spy = jest.spyOn(component.seleccionarProducto, 'emit');
    const lista = fixture.debugElement.queryAll(
      By.css('[data-testid="lista-seleccionar-producto"]')
    )[0];
    lista.nativeElement.click();
    expect(spy).toHaveBeenCalledWith(productos[0]);
  });

  it('debe desahabilitar el boton de crear pedido, cuando no haya productos seleccionados', () => {
    fixture.componentRef.setInput('cantidadProductosSeleccionados', 0);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-ver-carrito"]')
    );
    expect(boton.nativeElement.disabled).toBeTruthy();
  });

  it('debe habilitar el boton de crear pedido, cuando haya productos seleccionados', () => {
    fixture.componentRef.setInput('cantidadProductosSeleccionados', 1);
    fixture.detectChanges();
    const boton = fixture.debugElement.query(
      By.css('button[data-testid="boton-ver-carrito"]')
    );
    expect(boton.nativeElement.disabled).toBeFalsy();
  });

  it('debe emitir el evento "abrirModalCrearPedido" cuando se le de click al "boton-ver-carrito"', () => {
    fixture.componentRef.setInput('cantidadProductosSeleccionados', 1);
    fixture.detectChanges();
    const spy = jest.spyOn(component.abrirModalCrearPedido, 'emit');
    const lista = fixture.debugElement.queryAll(
      By.css('[data-testid="boton-ver-carrito"]')
    )[0];
    lista.nativeElement.click();
    expect(spy).toHaveBeenCalled();
  });
});
