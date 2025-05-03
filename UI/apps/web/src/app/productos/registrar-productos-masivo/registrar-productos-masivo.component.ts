import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import {
  Alerta,
  AlertaService,
  SharedModule,
  TipoAlerta,
  Utilidades,
} from '@storeflow/design-system';
import { Producto } from '../../app-model';
import { MensajesProductos } from '../productos.contantes';
import { EstadoCarga } from '../productos.enum';
import { RegistrarProducto, ResultadoCargaMasiva } from '../productos.model';
import { ProductosService } from '../productos.service';
import { ResultadoCargaMasivaComponent } from '../resultado-carga-masiva/resultado-carga-masiva.component';

@Component({
  selector: 'app-registrar-productos-masivo',
  standalone: true,
  imports: [SharedModule, ResultadoCargaMasivaComponent, CommonModule],
  providers: [ProductosService],
  templateUrl: './registrar-productos-masivo.component.html',
  styleUrl: './registrar-productos-masivo.component.scss',
})
export class RegistrarProductosMasivoComponent {
  service = inject(ProductosService);
  dialogRef = inject(MatDialogRef<RegistrarProductosMasivoComponent>);
  alertaService = inject(AlertaService);
  datosArchivo = signal<File>({} as File);
  estadoCargaEnum = EstadoCarga;
  estadoCarga = EstadoCarga.inicial;
  resultadoCarga = signal<ResultadoCargaMasiva>({
    productos: [] as Producto[],
  } as ResultadoCargaMasiva);

  get tamanioArchivo() {
    return Utilidades.obtenerTamanioArchivo(this.datosArchivo().size);
  }

  get estadoCargaEsCompletado() {
    return this.estadoCarga === EstadoCarga.completado;
  }

  get tieneProductosCargados() {
    return this.resultadoCarga().productos.length;
  }

  get alerta(): Alerta {
    return {
      tipo: TipoAlerta.Success,
      descricion: MensajesProductos.registroProductoExitoso,
    };
  }

  get productosAGuardar(): RegistrarProducto[] {
    return (
      this.resultadoCarga().productos.map((producto) => ({
        ...producto,
        fabricanteAsociado: producto.fabricanteAsociado.id,
      })) ?? []
    );
  }

  adjuntarArchivo(event: Event) {
    const elemento = event.target as HTMLInputElement;
    const archivo = elemento.files?.[0];
    if (archivo) {
      this.datosArchivo.set(archivo);
      this.estadoCarga = EstadoCarga.cargando;
      this.cargarProductosMasivoService(archivo);
    }
  }

  cargarProductosMasivoService(archivo: File) {
    this.service.cargarProductosMasivo(archivo).subscribe({
      next: (resultadoCarga) => {
        this.estadoCarga = EstadoCarga.completado;
        this.resultadoCarga.set(resultadoCarga);
      },
      error: () => {
        this.estadoCarga = EstadoCarga.inicial;
      },
    });
  }

  guardarProductosMasivos() {
    if (this.tieneProductosCargados) {
      this.service.guardarProductosMasivos(this.productosAGuardar).subscribe({
        next: () => {
          this.dialogRef.close();
          this.alertaService.abrirAlerta(this.alerta);
        },
      });
    }
  }
}
