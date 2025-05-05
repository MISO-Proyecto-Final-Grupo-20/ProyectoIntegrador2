import { Component, inject, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  Alerta,
  AlertaService,
  OpcionesLista,
  SharedModule,
  TipoAlerta,
} from '@storeflow/design-system';
import { Vendedor } from '../../app-model';
import { AppService } from '../../app.service';
import { MensajesPlanesVenta } from '../planes-venta.constantes';
import {
  FormularioPlanesVenta,
  RegistroPlanVenta,
  VendedorPlanVenta,
} from '../planes-venta.model';
import { PlanesVentaService } from '../planes-venta.service';

@Component({
  selector: 'app-registrar-planes-venta',
  standalone: true,
  imports: [SharedModule, ReactiveFormsModule],
  providers: [PlanesVentaService, AppService],
  templateUrl: './registrar-planes-venta.component.html',
  styleUrl: './registrar-planes-venta.component.scss',
})
export class RegistrarPlanesVentaComponent {
  private service = inject(PlanesVentaService);
  private appService = inject(AppService);
  alertaService = inject(AlertaService);
  periodosTiempo = signal<OpcionesLista>([]);
  vendedores = signal<Vendedor[]>([]);
  formulario = new FormGroup<FormularioPlanesVenta>({
    periodoTiempo: new FormControl(null, Validators.required),
    valorVentas: new FormControl(null, [
      Validators.required,
      Validators.min(0.01),
    ]),
    vendedores: new FormGroup({}),
  });

  get controlesVendedores() {
    return this.formulario.get('vendedores') as FormGroup;
  }

  get vendedoresSeleccionados(): VendedorPlanVenta[] {
    return this.vendedores()
      .filter(
        (vendedor) => this.controlesVendedores.controls[vendedor.id]?.value
      )
      .map(({ id, nombre }) => ({ id, nombre }));
  }

  get desactivarBoton() {
    return this.formulario.invalid || this.vendedoresSeleccionados.length === 0;
  }

  get alertaExitosa(): Alerta {
    return {
      tipo: TipoAlerta.Success,
      descricion: MensajesPlanesVenta.registroPlanesVentaExitoso,
    };
  }

  constructor() {
    this.obtenerPeriodosTiempo();
    this.obtenerVendedores();
  }

  obtenerPeriodosTiempo() {
    this.service.obtenerPeriodosTiempo().subscribe({
      next: (periodos) => {
        this.periodosTiempo.set(periodos);
      },
    });
  }

  obtenerVendedores() {
    this.appService.obtenerVendedores().subscribe({
      next: (vendedores) => {
        this.vendedores.set(vendedores);
        this.asignarControlesVendedores(vendedores);
      },
    });
  }

  asignarControlesVendedores(vendedores: Vendedor[]) {
    const grupoVendedores: { [key: number]: FormControl<boolean | null> } = {};

    vendedores.forEach((vendedor) => {
      grupoVendedores[vendedor.id] = new FormControl(false);
    });

    this.formulario.setControl('vendedores', new FormGroup(grupoVendedores));
  }

  registrarPlanVentas() {
    if (this.desactivarBoton) return;

    const planesVenta = {
      periodoTiempo: this.formulario.value.periodoTiempo,
      valorVentas: this.formulario.value.valorVentas,
      vendedores: this.vendedoresSeleccionados,
    } as RegistroPlanVenta;

    this.service.registrarPlanVentas(planesVenta).subscribe({
      next: () => {
        this.formulario.reset();
        this.alertaService.abrirAlerta(this.alertaExitosa);
      },
    });
  }
}
