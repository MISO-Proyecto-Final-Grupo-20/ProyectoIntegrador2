import { Producto, ProductoSeleccionado } from '../app.model';

export class UtilidadesCrearPedido {
  static obtenerProductoSeleccionado(
    productosSeleccionados: ProductoSeleccionado[],
    producto: ProductoSeleccionado
  ): ProductoSeleccionado[] {
    const index = productosSeleccionados.find(
      (seleccionado) => seleccionado.codigo === producto.codigo
    );
    if (index)
      return productosSeleccionados.filter(
        (seleccionado) => seleccionado.codigo !== producto.codigo
      );

    return [...productosSeleccionados, producto];
  }

  static obtenerProductosFiltradosConSeleccion(
    productosOriginales: Producto[],
    seleccionados: ProductoSeleccionado[]
  ): Producto[] {
    return productosOriginales.map((producto) => {
      const seleccionado = seleccionados.find(
        (seleccionado) => seleccionado.codigo === producto.codigo
      );
      return {
        ...producto,
        seleccionado: !!seleccionado,
      };
    });
  }

  static filtrarProductos(filtro: string, productos: Producto[]) {
    const normalizado = (filtro ?? '').trim().normalize().toLowerCase();
    if (!normalizado) return productos;

    return productos.filter(
      ({ nombre, codigo }) =>
        nombre.normalize().toLowerCase().includes(normalizado) ||
        codigo.normalize().toLowerCase().includes(normalizado)
    );
  }
}
