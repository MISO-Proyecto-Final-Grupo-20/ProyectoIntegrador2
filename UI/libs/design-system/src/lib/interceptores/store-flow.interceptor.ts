/* eslint-disable @typescript-eslint/no-explicit-any */
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { TipoAlerta } from '../components/alerta/alerta.model';
import { AlertaService } from '../components/alerta/alerta.service';

const UrlsSinIntercepcion = ['login'];

export const StoreFlowInterceptor = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const alertaService = inject(AlertaService);
  if (UrlsSinIntercepcion.some((url) => req.url.includes(url)))
    return next(req);

  return next(req).pipe(
    catchError((httpError: HttpErrorResponse) => {
      const mensaje = obtenerMensaje(httpError);
      alertaService.abrirAlerta({
        tipo: TipoAlerta.Danger,
        descricion: mensaje,
      });

      return throwError(() => new Error(httpError.message));
    })
  );
};

function obtenerMensaje(httpError: HttpErrorResponse): string {
  if (httpError.status !== 500 && httpError.status !== 0) {
    let error =
      httpError.error?.mensaje ?? httpError.error ?? httpError.message;

    if (error instanceof ArrayBuffer) {
      const decoder = new TextDecoder('utf-8');
      error = decoder.decode(error);
    }

    if (httpError.status === 404) {
      return error ?? httpError.statusText;
    }

    return error;
  } else {
    return 'Error no controlado';
  }
}
