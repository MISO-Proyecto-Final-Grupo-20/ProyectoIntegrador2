import { computed } from '@angular/core';
import { StateSignals, withComputed } from '@ngrx/signals';
import { Cliente, VendedoresState } from '../vendedores.model';

import { UtilidadesCrearPedido } from '../../shared/utilidades-crear-pedido';

export const selectorsStore = withComputed((store) => {
  const {
    clientes,
    filtroCliente,
    filtroProducto,
    productos,
    productosSeleccionados,
  } = store as StateSignals<VendedoresState>;
  const productosFiltrados = computed(() =>
    UtilidadesCrearPedido.filtrarProductos(filtroProducto(), productos())
  );
  return {
    clientesFiltrados: computed(() =>
      filtrarClientes(filtroCliente(), clientes())
    ),
    productosFiltrados,
    productosFiltradosConSeleccion: computed(() =>
      UtilidadesCrearPedido.obtenerProductosFiltradosConSeleccion(
        productosFiltrados(),
        productosSeleccionados()
      )
    ),
  };
});

function filtrarClientes(filtro: string, clientes: Cliente[]) {
  const normalizado = (filtro ?? '').trim().normalize().toLowerCase();
  if (!normalizado) return clientes;

  return clientes.filter(
    ({ nombre, id }) =>
      nombre.normalize().toLowerCase().includes(normalizado) ||
      id.toString().normalize().toLowerCase().includes(normalizado)
  );
}
