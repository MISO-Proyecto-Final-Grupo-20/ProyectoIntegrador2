// Base Jest configuration with coverage settings
const baseConfig = {
  coveragePathIgnorePatterns: [
    '/node_modules/',
    '/test/',
    '/*.spec.ts',
    '/environments/',
    '/mocks/',
    '/*.mock.ts',
    '/*.module.ts',
    '/generated/',
    '/assets/',
    '/locale/',
    '/dist/',
    '/.nx/',
    '/android/',
    '/ios/',
    '/www/',
    '/capacitor.config.ts',
    '/polyfills.ts',
    '/test-setup.ts',
    '/main.ts',
    '/index.html',
    '/styles.scss',
    '/*.routes.ts',
    '/*.constantes.ts',
    '/*.config.ts'
  ],
  collectCoverageFrom: [
    'src/**/*.ts',
    '!src/**/*.spec.ts',
    '!src/**/*.module.ts',
    '!src/environments/**',
    '!src/main.ts',
    '!src/polyfills.ts',
    '!src/test-setup.ts'
  ],
  coverageReporters: ['cobertura', 'lcov', 'text-summary'],
  coverageThreshold: {
    global: {
      branches: 40,
      functions: 40,
      lines: 40,
      statements: 40
    }
  }
};

export default baseConfig;
