import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { productosMock, vendedoresMock } from './app-mocks';
import { Producto, Vendedor } from './app-model';
import { AppsUrls } from './app.urls';

@Injectable()
export class AppService {
  private http = inject(HttpClient);

  obtenerVendedores(): Observable<Vendedor[]> {
    return this.http.get<Vendedor[]>(AppsUrls.vendedores);
    return of(vendedoresMock);
  }

  obtenerProductos(): Observable<Producto[]> {
    return this.http.get<Producto[]>(AppsUrls.productos);
    return of(productosMock);
  }
}
