import { productos } from '../app.mocks';
import { EntregaProgramada } from './clientes.model';

export const mocksEntregasProgramadas: EntregaProgramada[] = [
  {
    id: 1,
    numero: 7669,
    fechaEntrega: new Date(2025, 3, 22),
    lugarEntrega: 'Cr 238#47- 889',
    productos,
  },
  {
    id: 2,
    numero: 7668,
    fechaEntrega: new Date(2025, 3, 22),
    lugarEntrega: 'Cr 238#47- 889',
    productos,
  },
  {
    id: 3,
    numero: 7667,
    fechaEntrega: new Date(2025, 3, 22),
    lugarEntrega: 'Cr 238#47- 889',
    productos,
  },
  {
    id: 4,
    numero: 7666,
    fechaEntrega: new Date(2025, 3, 22),
    lugarEntrega: 'Cr 238#47- 889',
    productos,
  },
];
