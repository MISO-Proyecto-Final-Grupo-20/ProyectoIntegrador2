import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Cliente } from '../vendedores.model';
import { VendedoresUrls } from '../vendedores.urls';
import { mockClientes } from '../mock-vendedores';
import { Pedido, Producto, RegistroPedido } from '../../app.model';
import { AppsUrls } from '../../app.urls';
import { mockProductos, mocksPedidos } from '../../app.mocks';

@Injectable()
export class VendedoresService {
  private http = inject(HttpClient);

  obtenerClientes(): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(VendedoresUrls.obtenerClientes);
    return of(mockClientes);
  }

  validarInventarioProducto(producto: Producto): Observable<boolean> {
    return this.http.post<boolean>(
      AppsUrls.validarInventarioProducto,
      producto
    );
    return of(true);
  }

  obtenerProductos(): Observable<Producto[]> {
    return this.http.get<Producto[]>(AppsUrls.obtenerProductos);
    return of(mockProductos);
  }

  crearPedido(
    productos: RegistroPedido[],
    idCliente: number
  ): Observable<void> {
    const url = VendedoresUrls.crearPedidoCliente.replace(
      '[idCliente]',
      idCliente.toString()
    );
    return this.http.post<void>(url, productos);
    return of(void 0);
  }

  obtenerPedidosPendientesCliente(idCliente: number): Observable<Pedido[]> {
    const url = VendedoresUrls.obtenerPedidosPendientesCliente.replace(
      '[idCliente]',
      idCliente.toString()
    );
    return this.http.get<Pedido[]>(url);
    return of(mocksPedidos);
  }
}
