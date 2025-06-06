import { inject } from '@angular/core';
import { patchState, withMethods } from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { AlertaService, SignalsOf, TipoAlerta } from '@storeflow/design-system';
import { pipe, switchMap, tap } from 'rxjs';
import { MensajesAlertas } from '../../app.constantes';
import { ClientesState } from '../clientes.model';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { ClientesService } from '../services/clientes.service';
import { ProductoSeleccionado, RegistroPedido } from '../../app.model';

export const effectsStore = withMethods(
  (store: SignalsOf<Partial<ClientesState>>) => {
    const service = inject(ClientesService);
    const alertaService = inject(AlertaService);
    const modalAgregarProductoService = inject(ModalAgregarProductoService);

    const obtenerProductos = rxMethod<void>(
      pipe(
        switchMap(() =>
          service.obtenerProductos().pipe(
            tap((productos) =>
              patchState(store, {
                productos: [...productos],
              })
            )
          )
        )
      )
    );

    const validarInventarioProducto = rxMethod<ProductoSeleccionado>(
      pipe(
        switchMap((producto) =>
          service.validarInventarioProducto(producto).pipe(
            tap((existe) => {
              if (!existe) {
                return alertaService.abrirAlerta({
                  tipo: TipoAlerta.Danger,
                  descricion: `${MensajesAlertas.noHaySuficienteInventario} ${producto.nombre}`,
                });
              }
              modalAgregarProductoService.cerrarModal();
              effects.seleccionarProducto(producto);
            })
          )
        )
      )
    );

    const seleccionarProducto = rxMethod<ProductoSeleccionado>(
      pipe(
        tap((producto) => {
          const seleccionadosActuales = store.productosSeleccionados?.() ?? [];
          const nuevosSeleccionados = obtenerProductoSeleccionado(
            seleccionadosActuales,
            producto
          );
          patchState(store, {
            productosSeleccionados: nuevosSeleccionados,
          });
        })
      )
    );

    const crearPedido = rxMethod<RegistroPedido[]>(
      pipe(
        switchMap((productos) =>
          service.crearPedido(productos).pipe(
            tap(() => {
              patchState(store, {
                productosSeleccionados: [],
              });
              alertaService.abrirAlerta({
                tipo: TipoAlerta.Success,
                descricion: MensajesAlertas.pedidoCreado,
              });
            })
          )
        )
      )
    );

    const obtenerPedidos = rxMethod<void>(
      pipe(
        switchMap(() =>
          service
            .obtenerPedidos()
            .pipe(
              tap((pedidos) => patchState(store, { pedidos: [...pedidos] }))
            )
        )
      )
    );

    const obtenerEntregasProgramadas = rxMethod<void>(
      pipe(
        switchMap(() =>
          service.obtenerEntregasProgramadas().pipe(
            tap((entregas) =>
              patchState(store, {
                entregasProgramadas: [...entregas],
                filtroEntrega: '',
              })
            )
          )
        )
      )
    );

    const effects = {
      obtenerProductos,
      validarInventarioProducto,
      seleccionarProducto,
      crearPedido,
      obtenerPedidos,
      obtenerEntregasProgramadas,
    };

    return effects;
  }
);

function obtenerProductoSeleccionado(
  productosSeleccionados: ProductoSeleccionado[],
  producto: ProductoSeleccionado
): ProductoSeleccionado[] {
  const index = productosSeleccionados.findIndex(
    (seleccionado) => seleccionado.codigo === producto.codigo
  );
  if (index === 0)
    return productosSeleccionados.filter(
      (seleccionado) => seleccionado.codigo !== producto.codigo
    );

  return [...productosSeleccionados, producto];
}
