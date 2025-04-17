import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { RegistrarVendedor } from './vendedores.model';
import { VendedoresUrls } from './vendedores.urls';

@Injectable()
export class VendedoresService {
  http = inject(HttpClient);

  registrarVendedor(vendedor: RegistrarVendedor): Observable<void> {
    const token = localStorage.getItem('authToken');
    const headers = { Authorization: `${token}` };

    return this.http.post<void>(VendedoresUrls.registrarVendedor, vendedor);
  }
}
