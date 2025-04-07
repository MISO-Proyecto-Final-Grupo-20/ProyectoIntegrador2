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
      branches: 60,
      functions: 60,
      lines: 60,
      statements: 60
    }
  }
};

export default baseConfig;
