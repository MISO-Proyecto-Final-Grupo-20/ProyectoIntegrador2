import { Component, inject, input } from '@angular/core';
import { SharedModule } from '@storeflow/design-system';
import { Producto } from '../../app-model';
import { ModalMostrarErroresService } from '../modal-mostrar-errores/modal-mostrar-errores.service';
import { ResultadoCargaMasiva } from '../productos.model';

@Component({
  selector: 'app-resultado-carga-masiva',
  standalone: true,
  imports: [SharedModule],
  providers: [ModalMostrarErroresService],
  templateUrl: './resultado-carga-masiva.component.html',
  styleUrl: './resultado-carga-masiva.component.scss',
})
export class ResultadoCargaMasivaComponent {
  modalMostrarErroresService = inject(ModalMostrarErroresService);
  resultadoCarga = input<ResultadoCargaMasiva>({} as ResultadoCargaMasiva);

  get productos(): Producto[] {
    return this.resultadoCarga().productos;
  }

  get errores(): string[] {
    return this.resultadoCarga().errores;
  }

  get mensajeCantidadProductosCargados(): string {
    return $localize`:@@producto.masivo.cargadosCorrectamente:${this.productos.length} productos cargados correctamente`;
  }

  get mensajeCantidadErrores(): string {
    return $localize`:@@producto.masivo.erroresNoCargados: Ver ${this.errores.length} errores no cargados`;
  }

  abrirModalErrores() {
    this.modalMostrarErroresService.abrirModal(this.errores);
  }
}
