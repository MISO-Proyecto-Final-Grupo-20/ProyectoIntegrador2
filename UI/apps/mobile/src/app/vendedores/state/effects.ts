import { patchState, withMethods } from '@ngrx/signals';
import { AlertaService, SignalsOf, TipoAlerta } from '@storeflow/design-system';
import { RegistrarVisita, VendedoresState } from '../vendedores.model';
import { inject } from '@angular/core';
import { VendedoresService } from '../services/vendedores.service';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { EMPTY, pipe, switchMap, tap } from 'rxjs';
import { ProductoSeleccionado, RegistroPedido } from '../../app.model';
import { MensajesAlertas } from '../../app.constantes';
import { ModalAgregarProductoService } from '../modal-agregar-producto/modal-agregar-producto.service';
import { UtilidadesCrearPedido } from '../../shared/utilidades-crear-pedido';

export const effectsStore = withMethods(
  (store: SignalsOf<Partial<VendedoresState>>) => {
    const service = inject(VendedoresService);
    const alertaService = inject(AlertaService);
    const modalAgregarProductoService = inject(ModalAgregarProductoService);

    const obtenerClientes = rxMethod<void>(
      pipe(
        switchMap(() =>
          service.obtenerClientes().pipe(
            tap((clientes) =>
              patchState(store, {
                filtroCliente: '',
                clienteSeleccionado: null,
                productosSeleccionados: [],
                clientes: [...clientes],
              })
            )
          )
        )
      )
    );
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
          const nuevosSeleccionados =
            UtilidadesCrearPedido.obtenerProductoSeleccionado(
              seleccionadosActuales,
              producto
            );
          patchState(store, {
            productosSeleccionados: nuevosSeleccionados,
          });
        })
      )
    );

    const crearPedido = rxMethod<{
      productos: RegistroPedido[];
      idCliente: number;
    }>(
      pipe(
        switchMap(({ productos, idCliente }) =>
          service.crearPedido(productos, idCliente).pipe(
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

    const obtenerPedidosPendientesCliente = rxMethod<void>(
      pipe(
        switchMap(() => {
          const cliente = store.clienteSeleccionado?.();
          if (!cliente) return EMPTY;
          return service
            .obtenerPedidosPendientesCliente(cliente.id)
            .pipe(
              tap((pedidos) =>
                patchState(store, { pedidosPendientes: [...pedidos] })
              )
            );
        })
      )
    );

    const registrarVisita = rxMethod<RegistrarVisita>(
      pipe(
        switchMap((datosRegistrarVisita) => {
          const cliente = store.clienteSeleccionado?.();
          if (!cliente) return EMPTY;
          return service.registrarVisita(cliente.id, datosRegistrarVisita).pipe(
            tap(() => {
              alertaService.abrirAlerta({
                tipo: TipoAlerta.Success,
                descricion: MensajesAlertas.registroVisitaExitoso,
              });
              patchState(store, {
                archivoSeleccionado: null,
              });
              effects.obtenerRegistroVisitas();
            })
          );
        })
      )
    );

    const obtenerRegistroVisitas = rxMethod<void>(
      pipe(
        switchMap(() => {
          const cliente = store.clienteSeleccionado?.();
          if (!cliente) return EMPTY;
          return service.obtenerRegistroVisitas(cliente.id).pipe(
            tap((visitasRegistradas) =>
              patchState(store, {
                archivoSeleccionado: null,
                visitasRegistradas: [...visitasRegistradas],
              })
            )
          );
        })
      )
    );

    const obtenerRutasAsignadas = rxMethod<void>(
      pipe(
        switchMap(() =>
          service.obtenerRutasAsignadas().pipe(
            tap((rutasAsignadas) =>
              patchState(store, {
                filtroRutasAsignadas: new Date(),
                rutasAsignadas: [...rutasAsignadas],
              })
            )
          )
        )
      )
    );

    const effects = {
      obtenerClientes,
      obtenerProductos,
      validarInventarioProducto,
      seleccionarProducto,
      crearPedido,
      obtenerPedidosPendientesCliente,
      registrarVisita,
      obtenerRegistroVisitas,
      obtenerRutasAsignadas,
    };

    return effects;
  }
);
