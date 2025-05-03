import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { DateAdapter, provideNativeDateAdapter } from '@angular/material/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter } from '@angular/router';
import {
  AuthInterceptor,
  CalendarAdapter,
  StoreFlowInterceptor,
} from '@storeflow/design-system';
import { appRoutes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(appRoutes),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([StoreFlowInterceptor, AuthInterceptor])
    ),
    provideNativeDateAdapter(),
    { provide: DateAdapter, useClass: CalendarAdapter },
  ],
};
