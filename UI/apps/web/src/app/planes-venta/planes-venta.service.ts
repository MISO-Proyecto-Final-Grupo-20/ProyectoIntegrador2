import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OpcionesLista } from '@storeflow/design-system';
import { Observable, of } from 'rxjs';
import { periodosTiempoMock } from './planes-venta.mocks';
import { RegistroPlanVenta } from './planes-venta.model';
import { PlanesVentaUrls } from './planes-venta.urls';

@Injectable()
export class PlanesVentaService {
  constructor(private http: HttpClient) {}

  obtenerPeriodosTiempo(): Observable<OpcionesLista> {
    return this.http.get<OpcionesLista>(PlanesVentaUrls.obtenerPeriodosTiempo);
    return of(periodosTiempoMock);
  }

  registrarPlanVentas(registroVenta: RegistroPlanVenta): Observable<void> {
    return this.http.post<void>(PlanesVentaUrls.planesVenta, registroVenta);
    return of(void 0);
  }
}
