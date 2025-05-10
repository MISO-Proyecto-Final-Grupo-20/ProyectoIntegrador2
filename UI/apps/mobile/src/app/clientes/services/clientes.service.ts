import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ClientesUrls } from '../clientes.urls';
import { mockProductos, mocksPedidos } from '../../app.mocks';
import { Pedido, Producto, RegistroPedido } from '../../app.model';
import { AppsUrls } from '../../app.urls';
import { EntregaProgramada } from '../clientes.model';
import { mocksEntregasProgramadas } from '../clientesMocks';

@Injectable()
export class ClientesService {
  http = inject(HttpClient);

  obtenerProductos(): Observable<Producto[]> {
    return this.http.get<Producto[]>(AppsUrls.obtenerProductos);
    return of(mockProductos);
  }

  validarInventarioProducto(producto: Producto): Observable<boolean> {
    return this.http.post<boolean>(
      AppsUrls.validarInventarioProducto,
      producto
    );
    // return of(true);
  }

  crearPedido(productos: RegistroPedido[]): Observable<void> {
    return this.http.post<void>(ClientesUrls.crearPedido, productos);
    // return of(void 0);
  }

  obtenerPedidos(): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(ClientesUrls.obtenerPedidosPendientes);
    return of(mocksPedidos);
  }

  obtenerEntregasProgramadas(): Observable<EntregaProgramada[]> {
    return this.http.get<EntregaProgramada[]>(
      ClientesUrls.obtenerEntregasProgramadas
    );
    return of(mocksEntregasProgramadas);
  }
}
