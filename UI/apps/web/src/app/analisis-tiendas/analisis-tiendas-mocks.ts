import { AnalisisVisita } from './analisis-tiendas.model';

export const AnalisisTiendasMocks: AnalisisVisita[] = [
  {
    id: 3,
    cliente: '3',
    fecha: '2025-05-18T23:11:00Z',
    hora: '11:11',
    archivo: {
      nombre: 'estantesvacioscortos.mp4',
      tamanio: 5684972,
      url: 'https://pocstoreflow.blob.core.windows.net/visitas/visita_4a783b6a-8526-4e72-af86-f75527052666.mp4',
    },
    observaciones:
      'Agrupa productos por categorías claras como alimentos, limpieza, bebidas, usa señalización visible, coloca productos básicos en zonas de fácil acceso, productos premium al nivel de los ojos, organiza por frecuencia de compra, crea áreas temáticas como desayuno o snacks, evita zonas desordenadas.',
  },
  {
    id: 1,
    cliente: '3',
    fecha: '2025-05-11T20:09:23.234361Z',
    hora: '08:09',
    archivo: {
      nombre: 'Estantesvacios.mp4',
      tamanio: 16083841,
      url: 'https://pocstoreflow.blob.core.windows.net/visitas/visita_f994e9ea-72ce-4bc0-b4bc-6c164f5127f5.mp4',
    },
    observaciones:
      'Productos voluminosos de baja rotación como papel higiénico, cajas de pañales, paquetes de agua o refrescos, productos estacionales, exhibiciones de promociones especiales o artículos de precio alto que no requieren reposición frecuente.',
  },
  {
    id: 4,
    cliente: '3',
    fecha: '2025-05-10T18:09:00Z',
    hora: '06:09',
    archivo: {
      nombre: 'estantesvacioscortos.mp4',
      tamanio: 5684972,
      url: 'https://pocstoreflow.blob.core.windows.net/visitas/visita_c332cce1-79c5-4628-a620-1e6db001152f.mp4',
    },
    observaciones: '',
  },
  {
    id: 2,
    cliente: '3',
    fecha: '0001-01-01T00:00:00',
    hora: '12:00',
    archivo: {
      nombre: 'estantesvacioscortos.mp4',
      tamanio: 5684972,
      url: 'https://pocstoreflow.blob.core.windows.net/visitas/visita_82a9ac80-9ea3-4ad5-a3ed-5a3bfa651699.mp4',
    },
    observaciones:
      'Separa productos de limpieza y alimentos en áreas distintas, usa estanterías específicas para cada categoría, coloca señalización clara, organiza por frecuencia de compra y tipo de producto, destaca los alimentos perecederos y básicos en zonas visibles, evita mezcla para prevenir contaminación.',
  },
];
