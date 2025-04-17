import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ListadoFabricantes, Producto } from './productos.model';
import { ProductosUrls } from './productos.urls';

@Injectable()
export class ProductosService {
  http = inject(HttpClient);

  obtenerListadoFabricantes(): Observable<ListadoFabricantes[]> {
    return this.http.get<ListadoFabricantes[]>(
      ProductosUrls.obtenerListadoFabricantes
    );
    // return of([
    //   { id: 1, nombre: 'Fabricante 1' },
    //   { id: 2, nombre: 'Fabricante 2' },
    // ]);
  }

  guardarProducto(producto: Producto): Observable<void> {
    return this.http.post<void>(ProductosUrls.guardarProducto, producto);
    // return of(void 0);
  }
}
