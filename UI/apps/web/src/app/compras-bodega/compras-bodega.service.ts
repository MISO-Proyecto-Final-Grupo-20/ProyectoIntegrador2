import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { OpcionesLista } from '@storeflow/design-system';
import { Observable, of } from 'rxjs';
import { ComprasBodegaUrls } from './compra-bodega.urls';
import { RegistroCompraBodega } from './compras-bodega.model';

@Injectable()
export class ComprasBodegaService {
  private http = inject(HttpClient);

  obtenerListadoBodegas(): Observable<OpcionesLista> {
    return this.http.get<OpcionesLista>(
      ComprasBodegaUrls.obtenerListadoBodegas
    );
    return of([
      { id: 1, descripcion: 'Bodega 1' },
      { id: 2, descripcion: 'Bodega 2' },
    ]);
  }

  registrarCompraBodega(datosRegistro: RegistroCompraBodega): Observable<void> {
    return this.http.post<void>(ComprasBodegaUrls.bodegas, datosRegistro);
    return of(void 0);
  }
}
