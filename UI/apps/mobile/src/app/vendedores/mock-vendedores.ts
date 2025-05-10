import { Cliente, RutaAsignada, Visita } from './vendedores.model';

export const mockClientes: Cliente[] = [
  {
    id: 578,
    nombre: 'Surtimax',
    direccion: 'Cr 213 #56- 90 #578',
  },
  {
    id: 576,
    nombre: 'Autoservicio Laguna',
    direccion: 'Cr 213 #56- 90 #578',
  },
  {
    id: 573,
    nombre: 'Autoservicio chapinero',
    direccion: 'Cr 213 #56- 90 #578',
  },
  {
    id: 572,
    nombre: 'Fruver',
    direccion: 'Cr 213 #56- 90 #578',
  },
];

export const mockVisitas: Visita[] = [
  {
    id: 1,
    fecha: new Date('2023-10-01'),
    hora: '10:00',
    archivo: {
      nombre: 'visita1.pdf',
      tamanio: 3000,
    },
  },
  {
    id: 2,
    fecha: new Date('2023-10-02'),
    hora: '15:00',
    archivo: {
      nombre: 'visita2.pdf',
      tamanio: 3000,
    },
  },
];

export const mockRutasAsignadas: RutaAsignada[] = [
  {
    cliente: 'Surtimax',
    direccion: 'Cr 238#47- 889',
    fecha: new Date(2025, 5, 3),
  },
  {
    cliente: 'Autoservicio Laguna',
    direccion: 'Cr 238#47- 889',
    fecha: new Date(2025, 5, 4),
  },
  {
    cliente: 'Autoservicio chapinero',
    direccion: 'Cr 238#47- 889',
    fecha: new Date(2025, 5, 5),
  },
  {
    cliente: 'Fruver',
    direccion: 'Cr 238#47- 889',
    fecha: new Date(2025, 5, 4),
  },
];
