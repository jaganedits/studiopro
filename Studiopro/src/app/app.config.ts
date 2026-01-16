import { ApplicationConfig, ErrorHandler, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import Aura from '@primeng/themes/aura';
import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { loggingInterceptor, networkInterceptor, authInterceptor, loadingInterceptor, retryInterceptor, cachingInterceptor, errorInterceptor } from './services/interceptors';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ConfirmService, ErrorHandlerService } from './services/helpers';
import { providePrimeNG } from 'primeng/config';
import { definePreset } from '@primeng/themes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

const MyPreset = definePreset(Aura, {
  semantic: {
    primary: {
        50: '{gray.50}',
        100: '{gray.100}',
        200: '{gray.200}',
        300: '{gray.300}',
        400: '{gray.400}',
        500: '{gray.500}',
        600: '{gray.600}',
        700: '{gray.700}',
        800: '{gray.800}',
        900: '{gray.900}',
        950: '{gray.950}'
    }
  }
});

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: MyPreset,
        options: {
          darkModeSelector: '.dark-mode',
          cssLayer: {
            name: 'primeng',
            order: 'tailwind-base, primeng, tailwind-utilities',
          },
        },
      },
      ripple: true,
    }),
    provideHttpClient(
      withFetch(), // Use fetch API for better performance
      withInterceptors([
        loadingInterceptor,    // 4. Show loading indicator
        loggingInterceptor,    // 1. Log requests (dev only)
        networkInterceptor,    // 2. Check network connectivity
        authInterceptor,       // 3. Add auth headers
        retryInterceptor,      // 5. Retry failed requests
        cachingInterceptor,    // 6. Cache GET requests
        errorInterceptor,      // 7. Handle errors
      ])
    ),
    MessageService,
    ConfirmationService,
    ConfirmService,
    ErrorHandlerService,

    // Custom error handler
    { provide: ErrorHandler, useClass: ErrorHandlerService },
  ]
};

