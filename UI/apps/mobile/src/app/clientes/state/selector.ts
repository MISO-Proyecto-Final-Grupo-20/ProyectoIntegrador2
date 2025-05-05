import { computed } from '@angular/core';
import { StateSignals, withComputed } from '@ngrx/signals';
import { ClientesState } from '../clientes.model';
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
  };
});

function filtrarPedidos(filtro: string | null, pedidos: Pedido[]) {
  const normalizado = (filtro ?? '').trim().normalize().toLowerCase();
  if (!normalizado) return pedidos;

  return pedidos.filter(({ numero }) =>
    numero.toString().normalize().toLowerCase().includes(normalizado)
  );
}
