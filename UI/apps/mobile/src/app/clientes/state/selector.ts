import { computed } from '@angular/core';
import { StateSignals, withComputed } from '@ngrx/signals';
import {
  ClientesState,
  Producto,
  ProductoSeleccionado,
} from '../clientes.model';
import { EstadoPedido } from '../../app.enum';
import { Pedido } from '../../app.model';

export const selectorsStore = withComputed((store) => {
  const {
    productos,
    filtroProducto,
    productosSeleccionados,
    pedidos,
    filtroPedido,
  } = store as StateSignals<ClientesState>;
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
    pedidosPendientes: computed(() =>
      pedidos().filter(({ estado }) => estado === EstadoPedido.pendiente)
    ),
    pedidosFiltrados: computed(() => filtrarPedidos(filtroPedido(), pedidos())),
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

function filtrarPedidos(filtro: string | null, pedidos: Pedido[]) {
  const normalizado = (filtro ?? '').trim().normalize().toLowerCase();
  if (!normalizado) return pedidos;

  return pedidos.filter(({ numero }) =>
    numero.toString().normalize().toLowerCase().includes(normalizado)
  );
}
