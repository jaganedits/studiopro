import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { ErrorHandlerService } from '../helpers/error-handler.service';
import { SKIP_ERROR_HANDLING } from './interceptor.tokens';

/**
 * Error Handling Interceptor
 * - Centralizes HTTP error handling
 * - Maps HTTP status codes to user-friendly messages
 * - Redirects to error pages for auth errors (401/403)
 * - Supports skipping via context token
 */
export const errorInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  const errorService = inject(ErrorHandlerService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Skip error handling if explicitly requested (let component handle)
      if (req.context.get(SKIP_ERROR_HANDLING)) {
        return throwError(() => error);
      }

      // Handle different error types
      if (error.status === 0) {
        // Network error or CORS issue
        errorService.handleNetworkError(error);
      } else if (error.status === 401) {
        // Unauthorized - redirect to unauthorized page
        router.navigate(['/unauthorized']);
      } else if (error.status === 403) {
        // Forbidden - redirect to unauthorized page with code
        router.navigate(['/unauthorized'], { queryParams: { code: '403' } });
      } else if (error.status === 404) {
        // Not found - redirect to 404 page
        router.navigate(['/not-found']);
      } else if (error.status >= 400 && error.status < 500) {
        // Other client errors (400-499)
        errorService.handleClientError(error);
      } else if (error.status >= 500) {
        // Server errors (500+)
        errorService.handleServerError(error);
      }

      return throwError(() => error);
    })
  );
};