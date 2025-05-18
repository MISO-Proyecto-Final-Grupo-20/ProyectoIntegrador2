import { HttpClient } from '@angular/common/http';
import { Capacitor } from '@capacitor/core';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import {
  Cliente,
  RegistrarVisita,
  RutaAsignada,
  Visita,
} from '../vendedores.model';
import { VendedoresUrls } from '../vendedores.urls';
import {
  mockClientes,
  mockRutasAsignadas,
  mockVisitas,
} from '../mock-vendedores';
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

  // registrarVisita(
  //   idCliente: number,
  //   visita: RegistrarVisita
  // ): Observable<void> {
  //   const url = VendedoresUrls.visitas.replace(
  //     '[idCliente]',
  //     idCliente.toString()
  //   );
  //   const formData = new FormData();
  //   formData.append('fecha', visita.fecha.toString());
  //   formData.append('hora', visita.hora);
  //   formData.append('archivo', visita.archivo);
  //   return this.http.post<void>(url, formData);
  //   return of(void 0);
  // }
  
  registrarVisita(idCliente: number, visita: RegistrarVisita): Observable<void> {
  const url = VendedoresUrls.visitas.replace('[idCliente]', idCliente.toString());
  const formData = new FormData();
  formData.append('fecha', visita.fecha.toString());
  formData.append('hora', visita.hora);
  formData.append('archivo', visita.archivo);

  // Detectar si estás en móvil nativo (Android/iOS)
  if (Capacitor.isNativePlatform()) {
    // Usar fetch para evitar que Capacitor intercepte la llamada
    const token = localStorage.getItem('store_token');
    return new Observable<void>((observer) => {
      fetch(url, {
        method: 'POST',
        body: formData,
        headers: {
          Authorization: token ?? '' // si tienes token
        }
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
          }
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error('Error al registrar visita con fetch:', error);
          observer.error(error);
        });
    });
  }

  // En navegador, usar HttpClient normalmente
  return this.http.post<void>(url, formData);
}

  obtenerRegistroVisitas(idCliente: number): Observable<Visita[]> {
    const url = VendedoresUrls.visitas.replace(
      '[idCliente]',
      idCliente.toString()
    );
    return this.http.get<Visita[]>(url);
    return of(mockVisitas);
  }

  obtenerRutasAsignadas(): Observable<RutaAsignada[]> {
    return this.http.get<RutaAsignada[]>(VendedoresUrls.obtenerRutasAsignadas);
    return of(mockRutasAsignadas);
  }
}
