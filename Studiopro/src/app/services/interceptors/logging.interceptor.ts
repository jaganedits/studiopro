import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoggerService } from '../helpers/logger.service';

/**
 * Logging Interceptor
 * - Logs all HTTP requests and responses (dev mode only)
 * - Tracks request duration
 * - Useful for debugging
 */
export const loggingInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  // Skip logging in production
  if (environment.production) {
    return next(req);
  }

  const logger = inject(LoggerService);
  const startTime = Date.now();

  logger.log(`[HTTP] ${req.method} ${req.url}`, 'color: cyan', {
    body: req.body,
    headers: req.headers.keys().reduce((acc, key) => {
      acc[key] = req.headers.get(key);
      return acc;
    }, {} as Record<string, string | null>),
  });

  return next(req).pipe(
    tap({
      next: (event) => {
        if (event instanceof HttpResponse) {
          const duration = Date.now() - startTime;
          logger.log(
            `[HTTP] ${req.method} ${req.url} - ${event.status} (${duration}ms)`,
            'color: limegreen',
            event.body
          );
        }
      },
      error: (error: HttpErrorResponse) => {
        const duration = Date.now() - startTime;
        logger.error(
          `[HTTP] ${req.method} ${req.url} - ${error.status} (${duration}ms)`,
          error.message
        );
      },
    })
  );
};