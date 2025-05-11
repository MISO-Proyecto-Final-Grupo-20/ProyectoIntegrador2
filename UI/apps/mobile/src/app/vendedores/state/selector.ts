import { computed } from '@angular/core';
import { StateSignals, withComputed } from '@ngrx/signals';
import { Cliente, RutaAsignada, VendedoresState } from '../vendedores.model';

import { UtilidadesCrearPedido } from '../../shared/utilidades-crear-pedido';

export const selectorsStore = withComputed((store) => {
  const {
    clientes,
    filtroCliente,
    filtroProducto,
    productos,
    productosSeleccionados,
    filtroRutasAsignadas,
    rutasAsignadas,
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
    rutasAsignadasPorFecha: computed(() =>
      filtrarRutasAsignadasPorFecha(filtroRutasAsignadas(), rutasAsignadas())
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

function filtrarRutasAsignadasPorFecha(
  filtroFecha: Date | null,
  rutasAsignadas: RutaAsignada[]
) {
  return rutasAsignadas.filter(({ fecha }) => {
    if (!filtroFecha) return true;
    const fechaAsignada = new Date(fecha);
    const fechaFiltro = new Date(filtroFecha);

    return fechaAsignada.toDateString() === fechaFiltro.toDateString();
  });
}
