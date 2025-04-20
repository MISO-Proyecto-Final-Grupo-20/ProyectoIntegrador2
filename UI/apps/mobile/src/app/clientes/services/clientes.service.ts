import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Producto, RegistroPedido } from '../clientes.model';
import { ClientesUrls } from '../clientes.urls';
import { mockProductos } from '../mocks-clientes';

@Injectable()
export class ClientesService {
  http = inject(HttpClient);

  obtenerProductos(): Observable<Producto[]> {
    return this.http.get<Producto[]>(ClientesUrls.obtenerProductos);
    // return of(mockProductos);
  }

  validarInventarioProducto(producto: Producto): Observable<boolean> {
    return this.http.post<boolean>(
      ClientesUrls.validarInventarioProducto,
      producto
    );
    // return of(true);
  }

  crearPedido(productos: RegistroPedido[]): Observable<void> {
    return this.http.post<void>(ClientesUrls.crearPedido, productos);
    // return of(void 0);
  }
}
