import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { delay, Observable, of } from 'rxjs';
import { ListadoFabricantes } from '../app-model';
import { productosSimulados } from './productos.mock';
import { RegistrarProducto, ResultadoCargaMasiva } from './productos.model';
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

  guardarProducto(producto: RegistrarProducto): Observable<void> {
    return this.http.post<void>(ProductosUrls.guardarProducto, producto);
    // return of(void 0);
  }

  cargarProductosMasivo(archivo: File): Observable<ResultadoCargaMasiva> {
    const formData = new FormData();
    formData.append('file', archivo);
    return this.http.post<ResultadoCargaMasiva>(
      ProductosUrls.cargarProductosMasivo,
      formData
    );

    return of(productosSimulados).pipe(delay(5000));
  }

  guardarProductosMasivos(productos: RegistrarProducto[]) {
    return this.http.post<void>(
      ProductosUrls.guardarProductosMasivos,
      productos
    );
    return of(void 0);
  }
}
