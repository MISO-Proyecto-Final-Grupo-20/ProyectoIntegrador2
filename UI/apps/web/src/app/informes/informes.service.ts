import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ConsultaInforme, Informe } from './informes.model';
import { InformesUrls } from './informes.urls';
import { informesMock } from './informes.mocks';

export class InformesService {
  private http = inject(HttpClient);

  consultarInformes(datosConsulta: ConsultaInforme): Observable<Informe[]> {
    return this.http.post<Informe[]>(
      InformesUrls.consultaInformes,
      datosConsulta
    );
    return of(informesMock);
  }
}
