import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, tap, throwError } from 'rxjs';
import { NetworkService } from '../helpers/network.service';

/**
 * Network Interceptor
 * - Checks internet connectivity before requests
 * - Tracks server reachability status
 * - Provides offline detection
 */
export const networkInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  const networkService = inject(NetworkService);

  // Check if online before making request
  if (!networkService.isOnline) {
    return throwError(() => new Error('No internet connection. Please check your network and try again.'));
  }

  return next(req).pipe(
    tap({
      next: () => {
        // Request successful - server is reachable
        networkService.setServerReachable();
      },
    }),
    catchError((error: HttpErrorResponse) => {
      if (error.status === 0) {
        // Network error - server unreachable (status 0 indicates network failure)
        networkService.setServerUnreachable();
      }
      return throwError(() => error);
    })
  );
};