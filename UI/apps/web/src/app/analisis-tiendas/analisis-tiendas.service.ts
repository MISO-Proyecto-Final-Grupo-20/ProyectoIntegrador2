import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AnalisisVisita } from './analisis-tiendas.model';
import { Observable, of } from 'rxjs';
import { AnalisisTiendasUrls } from './analisis-tiendas.urls';
import { AnalisisTiendasMocks } from './analisis-tiendas-mocks';

@Injectable()
export class AnalisisTiendasService {
  private http = inject(HttpClient);

  obtenerAnalisisVisitas(): Observable<AnalisisVisita[]> {
    // return this.http.get<AnalisisVisita[]>(
    //   AnalisisTiendasUrls.obtenerAnalisisVisitas
    // );
    return of(AnalisisTiendasMocks);
  }

  guardarObservaciones(
    idVisita: number,
    observaciones: string
  ): Observable<void> {
    const url = AnalisisTiendasUrls.guardarObsevacionesAnalisisVisitas.replace(
      '[idVisita]',
      idVisita.toString()
    );
    return this.http.post<void>(url, observaciones);
    return of(void 0);
  }
}
