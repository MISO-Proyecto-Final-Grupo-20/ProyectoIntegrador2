import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { productosMock, vendedoresMock } from './app-mocks';
import { ListadoFabricantes, Producto, Vendedor } from './app-model';
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

  obtenerListadoFabricantes(): Observable<ListadoFabricantes[]> {
    return this.http.get<ListadoFabricantes[]>(
      AppsUrls.obtenerListadoFabricantes
    );
    return of([
      { id: 1, nombre: 'Fabricante 1' },
      { id: 2, nombre: 'Fabricante 2' },
    ]);
  }
}
