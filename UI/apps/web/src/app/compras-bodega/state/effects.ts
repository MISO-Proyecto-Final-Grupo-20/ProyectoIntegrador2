import { inject } from '@angular/core';
import { patchState, withMethods } from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { AlertaService, SignalsOf, TipoAlerta } from '@storeflow/design-system';
import { pipe, switchMap, tap } from 'rxjs';
import {
  ComprasBodegaState,
  RegistroCompraBodega,
} from '../compras-bodega.model';
import { ComprasBodegaService } from '../compras-bodega.service';
import { AppService } from '../../app.service';
import { MensajesComprasBodegas } from '../compras-bodega.constantes';

export const effectsStore = withMethods(
  (store: SignalsOf<Partial<ComprasBodegaState>>) => {
    const service = inject(ComprasBodegaService);
    const appService = inject(AppService);
    const alertaService = inject(AlertaService);

    const cargarDatos = rxMethod<void>(
      pipe(
        tap(() => {
          effects.obtenerListadoFabricantes();
          effects.obtenerListadoBodegas();
          effects.obtenerProductos();
        })
      )
    );

    const obtenerListadoFabricantes = rxMethod<void>(
      pipe(
        switchMap(() =>
          appService.obtenerListadoFabricantes().pipe(
            tap((fabricantes) =>
              patchState(store, {
                listadoFabricantes: [...fabricantes],
              })
            )
          )
        )
      )
    );

    const obtenerListadoBodegas = rxMethod<void>(
      pipe(
        switchMap(() =>
          service.obtenerListadoBodegas().pipe(
            tap((bodegas) =>
              patchState(store, {
                listadoBodegas: [...bodegas],
              })
            )
          )
        )
      )
    );

    const obtenerProductos = rxMethod<void>(
      pipe(
        switchMap(() =>
          appService.obtenerProductos().pipe(
            tap((productos) =>
              patchState(store, {
                productos: [...productos],
              })
            )
          )
        )
      )
    );

    const registrarCompraBodega = rxMethod<RegistroCompraBodega>(
      pipe(
        switchMap((datosRegistroBodega) =>
          service.registrarCompraBodega(datosRegistroBodega).pipe(
            tap(() => {
              alertaService.abrirAlerta({
                tipo: TipoAlerta.Success,
                descricion: MensajesComprasBodegas.registroCompraExitoso,
              });
              patchState(store, {
                productosSeleccionados: [],
              });
            })
          )
        )
      )
    );

    const effects = {
      obtenerListadoBodegas,
      obtenerListadoFabricantes,
      obtenerProductos,
      cargarDatos,
      registrarCompraBodega,
    };

    return effects;
  }
);
