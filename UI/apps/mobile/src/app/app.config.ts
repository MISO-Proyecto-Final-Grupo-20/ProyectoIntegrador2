import { registerLocaleData } from '@angular/common';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import localeEsCO from '@angular/common/locales/es-CO';
import {
  ApplicationConfig,
  LOCALE_ID,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter } from '@angular/router';
import {
  AuthInterceptor,
  StoreFlowInterceptor,
} from '@storeflow/design-system';
import { appRoutes } from './app.routes';

registerLocaleData(localeEsCO, 'es-CO');

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(appRoutes),
    provideAnimationsAsync(),
    { provide: LOCALE_ID, useValue: 'es-CO' },
    provideHttpClient(
      withInterceptors([StoreFlowInterceptor, AuthInterceptor])
    ),
  ],
};
