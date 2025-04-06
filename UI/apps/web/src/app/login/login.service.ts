import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { DatosIngreso } from '@storeflow/design-system';
import { LoginUrls } from './login.urls';

@Injectable()
export class LoginService {
  http = inject(HttpClient);

  ingresar(datosIngreso: DatosIngreso): Observable<{ token: string }> {
    // cambiar por el servicio real
    return this.http.post<{ token: string }>(LoginUrls.ingresar, datosIngreso);
    // return of({
    //   token:
    //     'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE0ODUxNDA5ODQsImlhdCI6MTQ4NTEzNzM4NCwiaXNzIjoiYWNtZS5jb20iLCJzdWIiOiIyOWFjMGMxOC0wYjRhLTQyY2YtODJmYy0wM2Q1NzAzMThhMWQiLCJhcHBsaWNhdGlvbklkIjoiNzkxMDM3MzQtOTdhYi00ZDFhLWFmMzctZTAwNmQwNWQyOTUyIiwicm9sZXMiOltdfQ.Mp0Pcwsz5VECK11Kf2ZZNF_SMKu5CgBeLN9ZOP04kZo',
    // });
  }
}
