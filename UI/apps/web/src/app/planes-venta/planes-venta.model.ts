import { FormControl, FormGroup } from '@angular/forms';

export interface FormularioPlanesVenta {
  periodoTiempo: FormControl<number | null>;
  valorVentas: FormControl<number | null>;
  vendedores: FormGroup<{
    [key: number]: FormControl<boolean | null>;
  }>;
}

export interface RegistroPlanVenta {
  periodoTiempo: number;
  valorVentas: number;
  vendedores: number[];
}
