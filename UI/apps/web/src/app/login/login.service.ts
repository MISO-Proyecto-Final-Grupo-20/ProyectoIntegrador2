import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { DatosIngreso } from '@storeflow/design-system';
import { LoginUrls } from './login.urls';

@Injectable()
export class LoginService {
  http = inject(HttpClient);

  ingresar(datosIngreso: DatosIngreso): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(LoginUrls.ingresar, datosIngreso);
  }
}
