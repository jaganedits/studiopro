import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { retry, timer } from 'rxjs';
import { RETRY_COUNT } from './interceptor.tokens';

/**
 * Retry Interceptor
 * - Automatically retries failed requests
 * - Uses exponential backoff strategy
 * - Configurable retry count via context token
 */
export const retryInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  const retryCount = req.context.get(RETRY_COUNT);

  // Skip retry for non-idempotent methods (POST, PATCH, DELETE)
  const nonIdempotentMethods = ['POST', 'PATCH', 'DELETE'];
  if (nonIdempotentMethods.includes(req.method.toUpperCase())) {
    return next(req);
  }

  return next(req).pipe(
    retry({
      count: retryCount,
      delay: (error: HttpErrorResponse, retryIndex: number) => {
        // Only retry on network errors or 5xx server errors
        if (error.status === 0 || error.status >= 500) {
          // Exponential backoff: 1s, 2s, 4s, etc.
          const delayMs = Math.pow(2, retryIndex - 1) * 1000;
          console.log(`Retry attempt ${retryIndex}/${retryCount} after ${delayMs}ms`);
          return timer(delayMs);
        }
        // Don't retry client errors (4xx)
        throw error;
      },
    })
  );
};