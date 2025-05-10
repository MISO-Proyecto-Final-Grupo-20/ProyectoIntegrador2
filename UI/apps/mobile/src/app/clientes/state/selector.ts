import { computed } from '@angular/core';
import { StateSignals, withComputed } from '@ngrx/signals';
import { ClientesState, EntregaProgramada } from '../clientes.model';
import { EstadoPedido } from '../../app.enum';
import { Pedido } from '../../app.model';
import { UtilidadesCrearPedido } from '../../shared/utilidades-crear-pedido';

export const selectorsStore = withComputed((store) => {
  const {
    productos,
    filtroProducto,
    productosSeleccionados,
    pedidos,
    filtroPedido,
    filtroEntrega,
    entregasProgramadas,
  } = store as StateSignals<ClientesState>;
  const productosFiltrados = computed(() =>
    UtilidadesCrearPedido.filtrarProductos(filtroProducto(), productos())
  );
  return {
    productosFiltrados,
    productosFiltradosConSeleccion: computed(() =>
      UtilidadesCrearPedido.obtenerProductosFiltradosConSeleccion(
        productosFiltrados(),
        productosSeleccionados()
      )
    ),
    pedidosPendientes: computed(() =>
      pedidos().filter(({ estado }) => estado === EstadoPedido.pendiente)
    ),
    pedidosFiltrados: computed(() => filtrarPedidos(filtroPedido(), pedidos())),
    entregasFiltradas: computed(() =>
      filtrarEntregas(filtroEntrega(), entregasProgramadas())
    ),
  };
});

function filtrarPorNumero<T>(
  filtro: string,
  datos: T[],
  obtenerNumero: (item: T) => string | number
): T[] {
  const normalizado = (filtro ?? '').trim().normalize().toLowerCase();
  if (!normalizado) return datos;
  return datos.filter((dato) =>
    obtenerNumero(dato)
      .toString()
      .normalize()
      .toLowerCase()
      .includes(normalizado)
  );
}
function filtrarPedidos(filtro: string, pedidos: Pedido[]) {
  return filtrarPorNumero(filtro, pedidos, (pedido) => pedido.numero);
}

function filtrarEntregas(filtro: string, entregas: EntregaProgramada[]) {
  return filtrarPorNumero(filtro, entregas, (entrega) => entrega.numero);
}
