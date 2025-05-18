# 5. Estrategia de pruebas

Esta sección describe el enfoque de validación implementado en StoreFlow, incluyendo las pruebas unitarias, de integración, y su ejecución tanto local como automatizada en CI/CD.

---

## 5.1 Estrategia general

StoreFlow aplica diferentes tipos de pruebas para asegurar la calidad de sus componentes:

- ✅ **Pruebas unitarias**:
  - En **.NET (Backend)** con `xUnit` y `NSubstitute`.
  - En **Angular (Frontend)** con `Jest`.

- ✅ **Pruebas de integración**:
  - Con `Cypress`, validando los flujos completos de negocio en la interfaz web.

- ⚙️ **Cobertura de código**:
  - Se mide en los pipelines y se genera reporte en formato `lcov` y `cobertura`.

---

## 5.2 Ejecución local

### Backend (.NET)

```bash
cd Backend
dotnet test
```

Frameworks utilizados: `xUnit`, `NSubstitute`.

---

### Frontend (Angular - UI)

#### Pruebas unitarias

```bash
cd UI
npm run test
```

#### Pruebas de integración (Cypress)

1. Levantar dependencias:

```bash
docker-compose up
```

2. Ejecutar pruebas:

```bash
cd UI
npm install
npm run cypress:open
```

---

## 5.3 Integración con CI/CD

Las pruebas se ejecutan automáticamente en cada `push` o `pull request` mediante GitHub Actions.

### Workflows destacados:

- **ci.yml**:
  - Ejecuta pruebas de backend en `.NET`.
  - Inicia servicios como RabbitMQ mediante `services:` en el job.
  - Genera cobertura con herramientas de .NET.

- **ci_ui.yml**:
  - Ejecuta pruebas unitarias y de integración de la carpeta `UI`.
  - Usa `matrix` para probar múltiples aplicaciones (`web`, `mobile`).
  - Mide cobertura con `jest --coverage`.

---

## 5.4 Medición de cobertura

- En Backend se usa `coverlet` para generar reportes de cobertura.
- En Frontend (`Jest`) se genera cobertura automáticamente en cada ejecución (`lcov` y `text-summary`).
- Los resultados son recolectados y visibles como parte del workflow en GitHub Actions.

---

## 5.5 Buenas prácticas implementadas

- Uso de `NSubstitute` para simular dependencias en pruebas unitarias.
- Aislamiento de infraestructura real (RabbitMQ, PostgreSQL) para pruebas.
- Separación entre pruebas unitarias e integración.
- Validación visual y por consola de los tests de Cypress.

[⬅️ Volver al índice](index.md)