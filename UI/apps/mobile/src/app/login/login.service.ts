import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

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
    return this.http.post<{ token: string }>(LoginUrls.ingresar, {
      datosIngreso,
      tipoCategoria,
    });
  }

  registrarCliente(cliente: Cliente): Observable<void> {
    return this.http.post<void>(LoginUrls.registrarCliente, cliente);
  }
}
