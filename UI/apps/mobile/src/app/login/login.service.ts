import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { DatosIngreso, TipoCategoria } from '@storeflow/design-system';
import { Cliente } from './login.model';
import { LoginUrls } from './login.urls';

@Injectable()
export class LoginService {
  http = inject(HttpClient);

  ingresar(
    datosIngreso: DatosIngreso,
    tipoCategoria: TipoCategoria
  ): Observable<{ token: string }> {
    //cambiar por el servicio real
    return this.http.post<{ token: string }>(LoginUrls.ingresar, {
      datosIngreso,
      tipoCategoria,
    });
    // return of({
    //   token:
    //     'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE0ODUxNDA5ODQsImlhdCI6MTQ4NTEzNzM4NCwiaXNzIjoiYWNtZS5jb20iLCJzdWIiOiIyOWFjMGMxOC0wYjRhLTQyY2YtODJmYy0wM2Q1NzAzMThhMWQiLCJhcHBsaWNhdGlvbklkIjoiNzkxMDM3MzQtOTdhYi00ZDFhLWFmMzctZTAwNmQwNWQyOTUyIiwicm9sZXMiOltdfQ.Mp0Pcwsz5VECK11Kf2ZZNF_SMKu5CgBeLN9ZOP04kZo',
    // });
  }

  registrarCliente(cliente: Cliente): Observable<void> {
    return this.http.post<void>(LoginUrls.registrarCliente, cliente);
    // return of(void 0);
  }
}
