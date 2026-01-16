import { HttpInterceptorFn, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoadingService } from '../helpers/loading.service';
import { SKIP_LOADING } from './interceptor.tokens';

/**
 * Loading Interceptor
 * - Shows/hides loading indicator automatically
 * - Tracks concurrent requests
 * - Supports skipping via context token
 */
export const loadingInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  const loadingService = inject(LoadingService);

  // Skip loading indicator if explicitly requested
  if (req.context.get(SKIP_LOADING)) {
    return next(req);
  }

  loadingService.show();

  return next(req).pipe(
    finalize(() => {
      loadingService.hide();
    })
  );
};