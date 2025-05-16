import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Fabricante } from './fabricante.model';
import { FabricantesUrls } from './fabricantes.urls';

@Injectable()
export class FabricantesService {
  http = inject(HttpClient);

  guardarFabricante(fabricante: Fabricante): Observable<void> {
    const token = localStorage.getItem('authToken');
    const headers = { Authorization: `${token}` };

    return this.http.post<void>(
      FabricantesUrls.registrarFabricante,
      fabricante,
      { headers }
    );
  }
}
