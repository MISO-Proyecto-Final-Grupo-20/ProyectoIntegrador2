import { ResultadoCargaMasiva } from './productos.model';
//borrar cuando se tenga las rutas
export const productosSimulados: ResultadoCargaMasiva = {
  errores: [
    'Alimentos SAS S.A.S.',
    'Alimentos SAS S.A.S.',
    'Alimentos SAS S.A.S.',
    'Alimentos SAS S.A.S.',
    'Alimentos SAS S.A.S.',
    'Alimentos SAS S.A.S.',
    'Alimentos SAS S.A.S.',
    'Alimentos SAS S.A.S.',
  ],
  productos: [
    {
      nombre: 'Paca de leche x12 unidades',
      fabricanteAsociado: { id: 2, nombre: 'Alquería S.A.' },
      codigo: 'A7X9B3Q5LZ82MND4VYKCJ6T1W0GFRP',
      imagen:
        'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
      precio: 1000,
    },
    {
      nombre: 'Chocolatinas JET x40 unidades',
      fabricanteAsociado: { id: 1, nombre: 'Grupo Nutresa S.A.' },
      codigo: 'MND4VYKCJ6T1W0GFRP',
      imagen:
        'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
      precio: 1000,
    },
    {
      nombre: 'Galletas Festival x 10 unidades',
      fabricanteAsociado: { id: 1, nombre: 'Grupo Nutresa S.A.' },
      codigo: 'A7X9B3Q5LZ82MND4VY',
      imagen:
        'https://www.alqueria.com.co/sites/default/files/2022-09/Alqueria_LecheEnteraLargaVida_1L.png',
      precio: 1000,
    },
  ],
};

export const fabricantesSimulados = [
  { id: 1, nombre: 'Fabricante 1' },
  { id: 2, nombre: 'Fabricante 2' },
];
