import { computed } from '@angular/core';
import { StateSignals, withComputed } from '@ngrx/signals';
import {
  ClientesState,
  Producto,
  ProductoSeleccionado,
} from '../clientes.model';

export const selectorsStore = withComputed((store) => {
  const { productos, filtroProducto, productosSeleccionados } =
    store as StateSignals<ClientesState>;
  const productosFiltrados = computed(() =>
    filtrarProductos(filtroProducto(), productos())
  );
  return {
    productosFiltrados,
    productosFiltradosConSeleccion: computed(() =>
      obtenerProductosFiltradosConSeleccion(
        productosFiltrados(),
        productosSeleccionados()
      )
    ),
  };
});

function obtenerProductosFiltradosConSeleccion(
  productosOriginales: Producto[],
  seleccionados: ProductoSeleccionado[]
): Producto[] {
  return productosOriginales.map((producto) => {
    const seleccionado = seleccionados.find(
      (seleccionado) => seleccionado.codigo === producto.codigo
    );
    return {
      ...producto,
      seleccionado: !!seleccionado,
    };
  });
}

function filtrarProductos(
  filtro: string | null | undefined,
  productos: Producto[]
) {
  const normalizado = (filtro ?? '').trim().normalize().toLowerCase();
  if (!normalizado) return productos;

  return productos.filter(
    ({ nombre, codigo }) =>
      nombre.normalize().toLowerCase().includes(normalizado) ||
      codigo.normalize().toLowerCase().includes(normalizado)
  );
}
